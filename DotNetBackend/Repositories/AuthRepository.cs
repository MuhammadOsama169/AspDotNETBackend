using DotNetBackend.Data;
using DotNetBackend.DTO.User;
using DotNetBackend.Models;
using DotNetBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;  
using Microsoft.Extensions.Configuration; 
using DotNetBackend.Services; 

namespace DotNetBackend.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly ApplicationDbContext    _db;
    private readonly IGenericRepository<User> _usersRepo;
    private readonly IOtpService              _otpService;
    private readonly IPasswordHasher<User>    _hasher;
    private readonly IConfiguration           _config;
    private readonly IRefreshTokenRepository _refreshRepo;
    
    public AuthRepository(
        ApplicationDbContext db,
        IGenericRepository<User> usersRepo,
        IOtpService otpService,
        IPasswordHasher<User> hasher,
        IConfiguration config,
        IRefreshTokenRepository refreshRepo)
    {
        _db               = db;
        _usersRepo        = usersRepo;
        _otpService       = otpService;
        _hasher           = hasher;
        _config           = config;
        _refreshRepo      = refreshRepo;
    }
    
    //
    public async Task<RegistrationResultDto> RegisterAsync(
        string name,
        string email,
        string password,
        CancellationToken ct = default
    )
    {
        // 1) Check duplicates
        var exists = (await _usersRepo.GetListAsync(u => u.Email == email, ct))
            .FirstOrDefault();
        if (exists != null)
        {
            // signal “email taken”
            return new RegistrationResultDto {
                UserId  = Guid.Empty,
                OtpCode = string.Empty
            };
        }

        // 2) Create & save user
        var user = new User {
            Id           = Guid.NewGuid(),
            Name         = name,
            Email        = email,
            PasswordHash = _hasher.HashPassword(null!, password)
        };
        await _usersRepo.AddAsync(user, ct);
        await _usersRepo.SaveChangesAsync(ct);

        // 3) Generate OTP
        var code = await _otpService.GenerateAsync(user.Id, ct);

        // 4) Return result
        return new RegistrationResultDto {
            UserId  = user.Id,
            OtpCode = code
        };
    }
    
    public async Task<LoginResultDto> LoginAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        // 1) lookup & verify user/password
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == email, ct);
        
  
        if (user == null ||
            _hasher.VerifyHashedPassword(user, user.PasswordHash, password)
            == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // 2) generate access + refresh tokens via the repo
        var accessToken  = GenerateJwtToken(user);
        var refreshToken = await _refreshRepo.CreateAsync(user, ct);

        // 3) return both to client
        return new LoginResultDto
        {
            Id           = user.Id,
            Name         = user.Name!,
            Email        = user.Email!,
            AccessToken  = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task LogoutAsync(Guid userId, string refreshToken, CancellationToken ct)
    {
        // 1) look up the matching, non-expired token
        var rt = await _refreshRepo.FindValidAsync(userId, refreshToken, ct);
        if (rt != null)
        {
            // 2) delete it
            await _refreshRepo.DeleteAsync(rt, ct);
        }
        // if rt == null, it was already expired or invalid—no-op
    }
    
    private string GenerateJwtToken(User user)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!);
        var creds    = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.Name!)
        };

        var jwt = new JwtSecurityToken(
            issuer:   _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:   claims,
            expires:  DateTime.UtcNow.AddHours(72),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

}