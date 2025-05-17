using DotNetBackend.Models;

namespace DotNetBackend.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
}