using BlogDAL.DTO;
using BlogDAL.Models;
using BlogDAL.Service;
using BlogDAL.UnitOfWork;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IUnitOfWork unitOfWork;
        private readonly ArticleService articleService;
        private IMapper mapper;
        public ArticleController(IUnitOfWork _unitOfWork, IMapper _mapper, ArticleService _articleService)
        {
            unitOfWork = _unitOfWork;
            articleService = _articleService;
            mapper = _mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articleDtos = await unitOfWork.GetRepository<Article>()
                                  .GetAllWithSelect(a => new ArticleGetAllDto
                                  {
                                      ArticleId = a.ArticleId,
                                      Title = a.Title,
                                      Content = a.Content,
                                      FullName = a.User.FullName,
                                      CreatedAt = a.CreatedAt,
                                      ArticleLikes = a.ArticleLikes.Count(),
                                      Comment = a.Comment.Count(),
                                      ImageUrl = a.ImageUrl,
                                      IsPublished = a.IsPublished
                                  });

            //For admin panel publish activate this
            //var articles = articleDtos.Where(x => x.IsPublished == true).ToList();
            return Ok(articleDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetNotPublishedArticles()
        {
            var articleDtos = await unitOfWork.GetRepository<Article>()
                               .GetAllWithSelect(a => new ArticleGetAllDto
                               {
                                   ArticleId = a.ArticleId,
                                   Title = a.Title,
                                   Content = a.Content,
                                   CreatedAt = a.CreatedAt,
                                   IsPublished = a.IsPublished,
                               });
            var articles = articleDtos.Where(x => x.IsPublished == false).ToList();
            return Ok(articles);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            //var article = await unitOfWork.GetRepository<Article>().GetById(x => x.ArticleId == id);
            var articleDto = await articleService.GetArticleDetailsById(id);
            if (articleDto == null)
            {
                return NotFound();
            }
            return Ok(articleDto);

        }

        [HttpPost]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> Create([FromBody] ArticleDto articleDto)
        {
            var categories = await unitOfWork.GetRepository<Category>().GetByPredicate(x => articleDto.CategoryIds.Contains(x.CategoryId));

            var tags = await unitOfWork.GetRepository<Tag>().GetByPredicate(x => articleDto.TagIds.Contains(x.TagId));

            var user = await unitOfWork.GetRepository<User>().GetById(x => x.Id == articleDto.UserId);

            var articles = mapper.Map<Article>(articleDto);

            articles.Categories = categories;
            articles.Tags = tags;
            articles.User = user;

            await unitOfWork.GetRepository<Article>().Add(articles);

            unitOfWork.SaveChanges();

            return Ok("Article Created");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await unitOfWork.GetRepository<Article>().Delete(x => x.ArticleId == id);
            unitOfWork.SaveChanges();
            return Ok("Article Deleted");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PublishArticle([FromBody] int articleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetById(x => x.ArticleId == articleId);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            article.IsPublished = true;
            unitOfWork.GetRepository<Article>().Update(article);
            await unitOfWork.SaveChangesAsync();

            return Ok("Article Published");
        }


    }
}
