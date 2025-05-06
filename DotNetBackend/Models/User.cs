using System.ComponentModel.DataAnnotations;


namespace DotNetBackend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "basic";

        // User hasMany Posts
        public ICollection<Post> ?Posts { get; set; }
        // User hasMany Workspaces but basic user should only have 1 and business 2
        //public ICollection<Workspace>? Workspaces { get; set; }

    }
}
