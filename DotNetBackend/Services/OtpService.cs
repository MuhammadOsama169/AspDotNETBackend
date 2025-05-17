using DotNetBackend.Models;
using DotNetBackend.Repositories.Interfaces;

namespace DotNetBackend.Services;

public class OtpService:  IOtpService
{
    private readonly IOtpRepository _otpRepo;
    private static readonly Random _rng = new();
    
    public OtpService(IOtpRepository repo)
    {
        _otpRepo = repo;
    }
    public async Task<string> GenerateAsync(Guid userId, CancellationToken ct = default)
    {
        //upper-bound 10000 so max is 9999 and lower bound is 1000 so random 4 digits
        var code = _rng.Next(1000, 10000).ToString();
        var otp = new OtpEntry
        {
            UserId    = userId,
            Code      = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            Used      = false
        };

        // Persist new OTP
        await _otpRepo.AddAsync(otp, ct);
        return code;
    }

    public async Task<bool> ValidateAsync(Guid userId, string code, CancellationToken ct = default)
    {
        // Fetch a matching, unused, unexpired OTP
        var entry = await _otpRepo.GetValidAsync(userId, code, ct);
        if (entry == null)
            return false;

        // Mark it used
        await _otpRepo.MarkUsedAsync(entry, ct);
        return true;
    }
}
