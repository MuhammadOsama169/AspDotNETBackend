using DotNetBackend.Models;

namespace DotNetBackend.Repositories.Interfaces;

public interface IOtpRepository
{
    Task AddAsync(OtpEntry otp, CancellationToken ct = default);
    Task<OtpEntry?> GetValidAsync(Guid userId, string code, CancellationToken ct = default);
    Task MarkUsedAsync(OtpEntry otp, CancellationToken ct = default);
}