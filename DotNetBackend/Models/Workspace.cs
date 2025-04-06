namespace DotNetBackend.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public int UserId { get; set; }
        public required User User { get; set; }

        public string? Setting { get; set; }
        public string? RefId { get; set; }
    }
}
