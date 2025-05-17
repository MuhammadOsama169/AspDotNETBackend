namespace DotNetBackend.DTO.User;

public class RefreshDto
{
    public Guid   UserId       { get; set; }
    public string RefreshToken { get; set; } = "";
}