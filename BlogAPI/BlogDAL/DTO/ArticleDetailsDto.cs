using BlogDAL.Models;

namespace BlogDAL.DTO
{
    public class ArticleDetailsDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<ArticleLike>? ArticleLikes { get; set; }
    }
}
