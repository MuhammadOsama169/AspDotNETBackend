using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.DTO.User
{
    public class UserRegisterationDto
    {
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
