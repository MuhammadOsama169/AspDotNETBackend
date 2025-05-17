using DotNetBackend.DTO.User;
using DotNetBackend.Models;
using DotNetBackend.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DotNetBackend.Features.Commands.Register;

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, RegistrationResultDto>
{
    private readonly IAuthRepository _authRepo;

    public RegisterUserCommandHandler(
        IAuthRepository auth)
    {
        _authRepo = auth;
    }
    
    
    public Task<RegistrationResultDto> Handle(
        RegisterUserCommand req,
        CancellationToken ct
    ) => _authRepo.RegisterAsync(req.Name, req.Email, req.Password, ct);
}