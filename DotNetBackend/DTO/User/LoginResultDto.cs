﻿public class LoginResultDto
{
    public Guid   Id           { get; set; }
    public string Name         { get; set; } = "";
    public string Email        { get; set; } = "";
    public string AccessToken  { get; set; } = "";
    public string RefreshToken { get; set; } = ""; 
}
