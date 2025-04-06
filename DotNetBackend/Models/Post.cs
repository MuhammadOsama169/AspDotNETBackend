using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        // Foreign key property pointing to the owning user
        //ceate UserId and also User User object for reshionship
        public Guid UserId { get; set; }

        //Post belongsTo User
        public required User User { get; set; }
    }
}
