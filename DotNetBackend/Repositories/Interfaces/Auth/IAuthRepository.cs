using DotNetBackend.DTO.User;

namespace DotNetBackend.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<RegistrationResultDto> RegisterAsync(
        string name,
        string email,
        string password,
        CancellationToken ct = default
    );
    
    Task<LoginResultDto> LoginAsync(string email, string password, CancellationToken ct);
    Task LogoutAsync(Guid userId, string refreshToken, CancellationToken ct);

}