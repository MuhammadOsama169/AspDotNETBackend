
using DotNetBackend.DTO.User;
using MediatR;

namespace DotNetBackend.Features.Commands.Register;

public record RegisterUserCommand(string Name, string Email, string Password)
    : IRequest<RegistrationResultDto>;