using DotNetBackend.DTO.User;
using MediatR;

namespace DotNetBackend.Features.Commands.Auth;

public class LogoutCommand : IRequest {  
    public Guid   UserId       { get; init; }
    public string RefreshToken { get; init; } = "";
}