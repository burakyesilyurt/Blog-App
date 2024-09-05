using System.ComponentModel.DataAnnotations.Schema;

namespace BlogClient.Models
{
    public class ArticleImage
    {
        public int ArticleImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
    }
}