using BlogClient.APIRoutes;
using BlogClient.Models;
using BlogClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogClient.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IApiService _apiService;
        private readonly ApiRoutesService _apiRoutesService;

        public AccountController(IApiService apiService, ApiRoutesService apiRoutesService)
        {
            _apiService = apiService;
            _apiRoutesService = apiRoutesService;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("AccessToken") != null)
            {
                return RedirectToAction("Index", "Article");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
            {
                userLogin.Password = "";
                return View(userLogin);
            }

            var loginRoute = _apiRoutesService.GetRoute("login");
            var getUserRoute = _apiRoutesService.GetRoute("Account.Get");

            var data = await _apiService.PostAsync<UserLogin, Token>(loginRoute.Route, userLogin);
            if (!data.Success)
            {
                ModelState.AddModelError("Error", data.ErrorMessage);
                return View();
            }
            if (!data.Success)
            {
                return HandleApiError(data.StatusCode);
            }
            HttpContext.Session.SetString("AccessToken", data.Data.AccessToken);

            var userData = await _apiService.GetAsync<User>(getUserRoute.Route);
            if (userData != null)
            {
                HttpContext.Session.SetString("FullName", userData.Data.FullName);
                HttpContext.Session.SetString("Role", userData.Data.Role);
                HttpContext.Session.SetString("Id", userData.Data.Id.ToString());
            }

            if (userData.Data.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Article");
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("AccessToken") != null)
            {
                return RedirectToAction("Index", "Article");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid || userRegister.Role == Role.Admin)
            {
                userRegister.PasswordHash = "";
                userRegister.ConfirmPassword = "";
                return View(userRegister);
            }
            var route = _apiRoutesService.GetRoute("register");
            var data = await _apiService.PostAsync<UserRegister, object>(route.Route, userRegister);
            if (data.Success == false)
            {
                ModelState.AddModelError("Error", data.ErrorMessage);
                return View();
            }
            if (!data.Success)
            {
                return HandleApiError(data.StatusCode);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
