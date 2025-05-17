using DotNetBackend.DTO.User;
using DotNetBackend.Repositories.Interfaces;
using MediatR;

namespace DotNetBackend.Features.Commands.Auth;

public class LogoutCommandHandler :IRequestHandler<LogoutCommand>
{
    private readonly IAuthRepository _authRepo;

    public LogoutCommandHandler(
        IAuthRepository auth)
    {
        _authRepo = auth;
    }
    public async Task<Unit> Handle(LogoutCommand req, CancellationToken ct)
    {
        await _authRepo.LogoutAsync(req.UserId, req.RefreshToken, ct);
        return Unit.Value;
    }
}