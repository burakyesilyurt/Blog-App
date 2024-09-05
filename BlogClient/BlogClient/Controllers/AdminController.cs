using BlogClient.APIRoutes;
using BlogClient.DTO;
using BlogClient.Models;
using BlogClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogClient.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IApiService _apiService;
        private readonly ApiRoutesService _apiRoutesService;

        public AdminController(IApiService apiService, ApiRoutesService apiRoutesService)
        {
            _apiService = apiService;
            _apiRoutesService = apiRoutesService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HttpContext.Session.GetString("AccessToken") == null || HttpContext.Session.GetString("Role") != "Admin")
            {
                context.Result = RedirectToAction("Index", "Article");
            }

            base.OnActionExecuting(context);
        }

        public async Task<IActionResult> Index()
        {
            var route = _apiRoutesService.GetRoute("Article.GetAll");
            var articles = await _apiService.GetAsync<List<ArticleDto>>(route.Route);
            if (!articles.Success)
            {
                return HandleApiError(articles.StatusCode);
            }
            return View(articles.Data);
        }

        public async Task<IActionResult> Users()
        {
            var route = _apiRoutesService.GetRoute("Account.GetAll");
            var users = await _apiService.GetAsync<List<User>>(route.Route);
            if (!users.Success)
            {
                return HandleApiError(users.StatusCode);
            }
            return View(users.Data);
        }
        public async Task<IActionResult> Categories()
        {
            var route = _apiRoutesService.GetRoute("Category.Get");
            var categories = await _apiService.GetAsync<List<CategoryDTO>>(route.Route);
            if (!categories.Success)
            {
                return HandleApiError(categories.StatusCode);
            }
            return View(categories.Data);
        }
        public async Task<IActionResult> Tags()
        {
            var route = _apiRoutesService.GetRoute("Tag.GetAll");
            var tags = await _apiService.GetAsync<List<TagDTO>>(route.Route);
            if (!tags.Success)
            {
                return HandleApiError(tags.StatusCode);
            }
            return View(tags.Data);
        }

        public async Task<IActionResult> ConfirmArticles()
        {
            var route = _apiRoutesService.GetRoute("Article.GetNotPublishedArticles");
            var articles = await _apiService.GetAsync<List<ArticleNotPublishedDto>>(route.Route);

            if (!articles.Success)
            {
                return HandleApiError(articles.StatusCode);
            }
            return View(articles.Data);
        }

        public async Task<IActionResult> PublishArticle(int articleId)
        {
            var route = _apiRoutesService.GetRoute("Article.Publish");
            var articles = await _apiService.PostAsync<int, string>(route.Route, articleId);

            if (!articles.Success)
            {
                return HandleApiError(articles.StatusCode);
            }
            return RedirectToAction("ConfirmArticles");
        }

        public async Task<IActionResult> DeleteArticle(int articleId)
        {
            var route = _apiRoutesService.GetRoute("Article.Delete");
            var article = await _apiService.DeleteAsync<int, string>(route.Route, articleId);

            if (!article.Success)
            {
                return HandleApiError(article.StatusCode);
            }
            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);
            }
            return RedirectToAction("ConfirmArticles");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTag(TagDTO tagDto)
        {
            var route = _apiRoutesService.GetRoute("Tag.Post");
            var tag = await _apiService.PostAsync<TagDTO, Tag>(route.Route, tagDto);

            if (!tag.Success)
            {
                return HandleApiError(tag.StatusCode);
            }

            return RedirectToAction("Tags");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO categoryDTO)
        {
            var route = _apiRoutesService.GetRoute("Category.Post");
            var category = await _apiService.PostAsync<CategoryDTO, Category>(route.Route, categoryDTO);

            if (!category.Success)
            {
                return HandleApiError(category.StatusCode);
            }

            return RedirectToAction("Categories");
        }
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var route = _apiRoutesService.GetRoute("Account.Delete");
            var article = await _apiService.DeleteAsync<int, string>(route.Route, userId);

            if (!article.Success)
            {
                return HandleApiError(article.StatusCode);
            }
            return RedirectToAction("Users");
        }
    }
}
