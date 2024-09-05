using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogDAL.Models
{
    //Not Used
    public class ArticleImage
    {
        public int ArticleImageId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        [JsonIgnore]
        public Article Article { get; set; }
    }
}