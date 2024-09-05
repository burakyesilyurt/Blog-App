using System.ComponentModel.DataAnnotations;

namespace BlogDAL.DTO
{
    public class UserRegisterDTO
    {

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Compare("PasswordHash")]
        public string? ConfirmPassword { get; set; }
        public Role Role { get; set; }

    }
    public enum Role
    {
        Admin,
        User,
        Author
    }
}
