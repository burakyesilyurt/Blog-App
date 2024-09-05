using BlogClient.APIRoutes;
using BlogClient.DTO;
using BlogClient.Models.ViewModels;
using BlogClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogClient.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IApiService _apiService;
        private readonly ApiRoutesService _apiRoutesService;

        public ArticleController(IApiService apiService, ApiRoutesService apiRoutesService)
        {
            _apiService = apiService;
            _apiRoutesService = apiRoutesService;
        }

        public async Task<IActionResult> Index()
        {
            var route = _apiRoutesService.GetRoute("Article.GetAll");
            var article = await _apiService.GetAsync<List<ArticleGetAllDto>>(route.Route);
            if (!article.Success)
            {
                return HandleApiError(article.StatusCode);
            }
            return View(article.Data);
        }

        public async Task<IActionResult> GetArticle(int id)
        {
            var articleRoute = _apiRoutesService.GetRoute("Article.Get");
            var url = $"{articleRoute.Route}/{id}";
            var article = await _apiService.GetAsync<ArticleDetailsDto>(url);
            if (!article.Success)
            {
                return HandleApiError(article.StatusCode);
            }

            var commentsRoute = _apiRoutesService.GetRoute("Comment.GetByArticleId");
            url = $"{commentsRoute.Route}/{id}";
            var comments = await _apiService.GetAsync<List<CommentWithUserDto>>(url);
            if (!comments.Success)
            {
                return HandleApiError(comments.StatusCode);
            }

            var vm = new SingleArticleViewModel()
            {
                articleDetails = article.Data,
                commentWithUsers = comments.Data
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AccessToken")))
            {
                return RedirectToAction("Login", "Account");
            }
            var categoryRoute = _apiRoutesService.GetRoute("Category.Get");
            var categoryData = await _apiService.GetAsync<List<CategoryDTO>>(categoryRoute.Route);
            if (!categoryData.Success)
            {
                return HandleApiError(categoryData.StatusCode);
            }

            var tagRoute = _apiRoutesService.GetRoute("Tag.GetAll");
            var tagData = await _apiService.GetAsync<List<TagDTO>>(tagRoute.Route);
            if (!tagData.Success)
            {
                return HandleApiError(tagData.StatusCode);
            }

            var vm = new CreatePostViewModel()
            {
                Categories = categoryData.Data,
                Tags = tagData.Data
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticlePostDto articleDto, IFormFile image)
        {

            if (!ModelState.IsValid)
            {
                return View(articleDto);
            }



            string imageUrl = null;
            if (image != null && image.Length > 0)
            {

                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var filePath = Path.Combine(uploads, Path.GetFileName(image.FileName));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/{Path.GetFileName(image.FileName)}";
                articleDto.ImageUrl = imageUrl;
            }

            string userIdString = HttpContext.Session.GetString("Id");

            if (int.TryParse(userIdString, out int userId))
            {
                articleDto.UserId = userId;
            }
            var route = _apiRoutesService.GetRoute("Article.Post");
            var data = await _apiService.PostAsync<ArticlePostDto, string>(route.Route, articleDto);

            if (!data.Success)
            {
                return HandleApiError(data.StatusCode);
            }
            return RedirectToAction("Index", "Article");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentDTO commentDTO)
        {
            if (commentDTO.Content == null)
            {
                RedirectToAction("Index", "Article");
            }
            string userIdString = HttpContext.Session.GetString("Id");

            if (int.TryParse(userIdString, out int userId))
            {
                commentDTO.UserId = userId;
            }

            var route = _apiRoutesService.GetRoute("Comment.Post");
            var data = await _apiService.PostAsync<CommentDTO, string>(route.Route, commentDTO);

            if (!data.Success)
            {
                return HandleApiError(data.StatusCode);
            }
            return RedirectToAction("GetArticle", "Article", new { id = commentDTO.ArticleId });
        }

        public async Task<IActionResult> ArticleLike(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AccessToken")))
            {
                return RedirectToAction("Login", "Account");
            }
            var articleLikeDto = new ArticleLikeDto();
            articleLikeDto.ArticleId = id;
            string userIdString = HttpContext.Session.GetString("Id");

            if (int.TryParse(userIdString, out int userId))
            {
                articleLikeDto.UserId = userId;
            }

            var articleRoute = _apiRoutesService.GetRoute("Comment.Like");
            var url = $"{articleRoute.Route}/{articleLikeDto.ArticleId}";
            var data = await _apiService.PostAsync<ArticleLikeDto, string>(url, articleLikeDto);

            if (data.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RedirectToAction("GetArticle", "Article", new { id = articleLikeDto.ArticleId });
            }

            if (!data.Success)
            {
                return HandleApiError(data.StatusCode);
            }
            return RedirectToAction("GetArticle", "Article", new { id = articleLikeDto.ArticleId });
        }
    }
}
