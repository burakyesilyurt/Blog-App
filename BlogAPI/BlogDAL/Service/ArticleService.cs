using BlogDAL.DTO;
using BlogDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogDAL.Service
{
    public class ArticleService : GenericService<Article>
    {
        public ArticleService(AppDbContext _context) : base(_context)
        {
        }

        public async Task<ArticleDetailsDto> GetArticleDetailsById(int articleId)
        {
            return await dbSet
                .Where(a => a.ArticleId == articleId)
                .Include(a => a.User)
                .Include(a => a.Categories)
                .Include(a => a.Tags)
                .Include(a => a.ArticleLikes)
                .Select(a => new ArticleDetailsDto
                {
                    ArticleId = a.ArticleId,
                    Title = a.Title,
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    FullName = a.User.FullName,
                    Categories = a.Categories,
                    Tags = a.Tags,
                    ArticleLikes = a.ArticleLikes,
                    ImageUrl = a.ImageUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CommentWithUserDto>> GetCommentsByArticleId(int articleId)
        {
            return await dbSet
                .Where(a => a.ArticleId == articleId)
                .SelectMany(a => a.Comment)
                .Include(c => c.User)
                .Select(c => new CommentWithUserDto
                {
                    CommentId = c.CommentId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                    FullName = c.User.FullName
                })
                .ToListAsync();
        }
    }
}
