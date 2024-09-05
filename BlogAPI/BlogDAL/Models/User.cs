using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogDAL.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public ICollection<ViewCount>? ViewCounts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<ArticleLike>? ArticleLikes { get; set; }
        public ICollection<Article>? Article { get; set; }
        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
