using System.ComponentModel.DataAnnotations;

namespace BlogClient.Models
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}|:<>?~\-=\[\]\\;',./]).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
