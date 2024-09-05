using BlogDAL.DTO;
using BlogDAL.Models;
using BlogDAL.Service;
using BlogDAL.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ArticleService articleService;
        private readonly UserManager<User> userManager;

        public CommentController(IUnitOfWork _unitOfWork, ArticleService _articleService, UserManager<User> _userManager)
        {
            unitOfWork = _unitOfWork;
            articleService = _articleService;
            userManager = _userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentDTO commentDto)
        {

            var user = await userManager.GetUserAsync(HttpContext.User);
            if (commentDto.UserId != user.Id)
            {
                return BadRequest();
            }
            var article = await unitOfWork.GetRepository<Article>().GetById(x => x.ArticleId == commentDto.ArticleId);
            //var user = await unitOfWork.GetRepository<User>().GetById(x => x.Id == commentDto.UserId);

            var comment = commentDto.Adapt<Comment>();
            comment.Article = article;
            comment.User = user;


            await unitOfWork.GetRepository<Comment>().Add(comment);

            unitOfWork.SaveChanges();

            return Ok("Comment Created");
        }
        [HttpGet("{articleId:int}")]
        public async Task<IActionResult> GetCommentFromArticle([FromRoute] int articleId)
        {
            var comments = await articleService.GetCommentsByArticleId(articleId);
            return Ok(comments);

        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await unitOfWork.GetRepository<Comment>().Delete(x => x.CommentId == id);
            return Ok("Comment Deleted");
        }


        [HttpPost("{id:int}")]
        public async Task<IActionResult> LikeArticle(ArticleLikeDto articleLikeDto)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var article = await unitOfWork.GetRepository<Article>().GetById(x => x.ArticleId == articleLikeDto.ArticleId);
            if (articleLikeDto.UserId != user.Id)
            {
                return BadRequest();
            }
            var articleLike = articleLikeDto.Adapt<ArticleLike>();
            articleLike.Article = article;
            articleLike.User = user;

            var existingLike = await unitOfWork.GetRepository<ArticleLike>().GetById(l => l.ArticleId == articleLikeDto.ArticleId && l.UserId == user.Id);
            if (existingLike != null)
            {
                return BadRequest("You have already liked this article");
            }


            await unitOfWork.GetRepository<ArticleLike>().Add(articleLike);
            unitOfWork.SaveChanges();

            return Ok("Article Liked");
        }
    }
}
