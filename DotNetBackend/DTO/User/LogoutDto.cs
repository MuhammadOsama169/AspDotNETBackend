namespace DotNetBackend.DTO.User;

public class LogoutDto
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; } = "";
}