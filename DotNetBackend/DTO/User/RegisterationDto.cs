using System.ComponentModel.DataAnnotations;
using MediatR;

namespace DotNetBackend.DTO.User
{
    public class RegistrationDto: IRequest<RegistrationDto>
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
