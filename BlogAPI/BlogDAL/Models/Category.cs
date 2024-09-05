using System.Text.Json.Serialization;

namespace BlogDAL.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Article>? Articles { get; set; }
    }
}