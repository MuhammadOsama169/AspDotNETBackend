using DotNetBackend.DTO.User;
using DotNetBackend.Repositories.Interfaces;
using MediatR;

namespace DotNetBackend.Features.Commands.Login;

public class LoginUserCommandHandler: IRequestHandler<LoginUserCommand, LoginResultDto>
{
    private readonly IAuthRepository _authRepo;

    public LoginUserCommandHandler(IAuthRepository authRepo)
        => _authRepo = authRepo;
    
    public Task<LoginResultDto> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        return _authRepo.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken
        );
    }
}