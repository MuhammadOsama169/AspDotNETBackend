
public interface IOtpService
{
    Task<string> GenerateAsync(Guid userId, CancellationToken ct = default);
    Task<bool>ValidateAsync(Guid userId, string code, CancellationToken ct = default);
}