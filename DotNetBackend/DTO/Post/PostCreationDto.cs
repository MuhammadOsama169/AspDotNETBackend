using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.DTO.Post
{
    public class PostCreationDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
