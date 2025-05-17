using DotNetBackend.DTO.User;
using MediatR;

namespace DotNetBackend.Features.Commands.Login;

public class LoginUserCommand : IRequest<LoginResultDto>
{
    public string Email    { get; init; } = "";
    public string Password { get; init; } = "";
}