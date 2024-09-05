using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogDAL.Models
{
    public class ArticleLike
    {
        [Key]
        public int ArticleLikeId { get; set; }
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        [JsonIgnore]
        public Article Article { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}