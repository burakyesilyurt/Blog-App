using BlogClient.APIRoutes;
using BlogClient.Models;
using BlogClient.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogClient.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiService _apiService;
        private readonly ApiRoutesService _apiRoutesService;

        public HomeController(ILogger<HomeController> logger, IApiService apiService, ApiRoutesService apiRoutesService)
        {
            _logger = logger;
            _apiService = apiService;
            _apiRoutesService = apiRoutesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
