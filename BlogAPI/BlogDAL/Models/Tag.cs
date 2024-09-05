using System.Text.Json.Serialization;

namespace BlogDAL.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Article>? Articles { get; set; }
    }
}
