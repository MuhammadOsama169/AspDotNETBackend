namespace DotNetBackend.Models
{
    public class Workspace
    {
        public Guid? RefId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }

        public string? Setting { get; set; }

    }
}
