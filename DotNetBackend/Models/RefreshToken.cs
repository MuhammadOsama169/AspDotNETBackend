namespace DotNetBackend.Models;

public class RefreshToken
{
    public Guid    Id           { get; set; } = Guid.NewGuid();
    public Guid    UserId       { get; set; }
    public string  TokenHash    { get; set; } = string.Empty;
    public DateTime ExpiresAt   { get; set; }
    public DateTime CreatedAt   { get; set; } = DateTime.UtcNow;
    public string? DeviceInfo   { get; set; }    // track browser/device
}
