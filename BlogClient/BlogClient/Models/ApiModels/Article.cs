using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogClient.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; } = false;
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<ViewCount>? ViewCounts { get; set; }
        public ICollection<Comment>? Comment { get; set; }
        public ICollection<ArticleLike>? ArticleLikes { get; set; }
        public ICollection<ArticleImage>? ArticleImages { get; set; }


    }
}