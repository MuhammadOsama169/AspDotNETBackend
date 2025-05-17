using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNetBackend.Data;
using DotNetBackend.Models;
using DotNetBackend.Repositories.Interfaces;

namespace DotNetBackend.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public RefreshTokenRepository(
            ApplicationDbContext db,
            IPasswordHasher<User> hasher)
        {
            _db     = db;
            _hasher = hasher;
        }

        public async Task<string> CreateAsync(User user, CancellationToken ct = default)
        {
            // 1) generate raw token
            var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            // 2) hash it
            var hash = _hasher.HashPassword(user, raw);

            var entity = new RefreshToken
            {
                UserId    = user.Id,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // e.g. 7d lifetime
                CreatedAt = DateTime.UtcNow
            };

            _db.RefreshTokens.Add(entity);
            await _db.SaveChangesAsync(ct);

            return raw;
        }

        public async Task<RefreshToken?> FindValidAsync(
            Guid userId, string rawToken, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            var candidates = await _db.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.ExpiresAt > now)
                .ToListAsync(ct);

            return candidates
                .FirstOrDefault(rt => 
                    _hasher.VerifyHashedPassword(null!, rt.TokenHash, rawToken)
                     != PasswordVerificationResult.Failed);
        }

        public async Task DeleteAsync(RefreshToken token, CancellationToken ct = default)
        {
            _db.RefreshTokens.Remove(token);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAllForUserAsync(Guid userId, CancellationToken ct = default)
        {
            var list = await _db.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync(ct);

            _db.RefreshTokens.RemoveRange(list);
            await _db.SaveChangesAsync(ct);
        }
    }
}
