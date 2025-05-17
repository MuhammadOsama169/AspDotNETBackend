
using DotNetBackend.Models;

namespace DotNetBackend.Repositories.Interfaces
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Creates a new refresh token row (hashed) and returns the raw token string.
        /// </summary>
        Task<string> CreateAsync(User user, CancellationToken ct = default);

        /// <summary>
        /// Finds a valid (not expired) refresh token by raw value (compares hash).
        /// </summary>
        Task<RefreshToken?> FindValidAsync(Guid userId, string rawToken, CancellationToken ct = default);

        /// <summary>
        /// Deletes a specific RefreshToken entity.
        /// </summary>
        Task DeleteAsync(RefreshToken token, CancellationToken ct = default);

        /// <summary>
        /// Deletes all refresh tokens for the given user.
        /// </summary>
        Task DeleteAllForUserAsync(Guid userId, CancellationToken ct = default);
    }
}