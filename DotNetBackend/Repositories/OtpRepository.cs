using DotNetBackend.Data;
using DotNetBackend.Models;
using DotNetBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotNetBackend.Repositories;

public class OtpRepository :IOtpRepository
{
    private readonly ApplicationDbContext _db;
    public OtpRepository(ApplicationDbContext db) => _db = db;
    
    public async Task AddAsync(OtpEntry otp, CancellationToken ct = default)
    {
        await _db.Otps.AddAsync(otp, ct);
        await _db.SaveChangesAsync(ct);
    }

    public Task<OtpEntry?> GetValidAsync(Guid userId, string code, CancellationToken ct = default) =>
        _db.Otps
            .Where(o => o.UserId == userId && !o.Used && o.ExpiresAt > DateTime.UtcNow && o.Code == code)
            .FirstOrDefaultAsync(ct);

    public async Task MarkUsedAsync(OtpEntry otp, CancellationToken ct = default)
    {
        otp.Used = true;
        _db.Otps.Update(otp);
        await _db.SaveChangesAsync(ct);
    }
}