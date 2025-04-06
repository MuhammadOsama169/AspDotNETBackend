

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetBackend.Data;
using DotNetBackend.DTO.User;
using DotNetBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController :ControllerBase 
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "A user with that email already exists." });
            }


            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email
            };

            // Hash the password using PasswordHasher.
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate a JWT token for the new user.
            var token = GenerateJwtToken(user);

            var response = new
            {
                user = new { user.Id, user.Name, user.Email },
                token = token,
                message = "User registered successfully."
            };

            return Ok(response);
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the user by email.
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            // Verify the password.
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Invalid email or password." });

            // Generate JWT token.
            var token = GenerateJwtToken(user);
            var response = new
            {
                user = new { user.Id, user.Name, user.Email },
                token = token,
                message = "User logged in successfully."
            };

            return Ok(response);
        }

        // POST: api/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {

            return Ok(new { message = "Logged out successfully." });
        }

        // -------------------- //
        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:SecretKey"] ?? throw new Exception("JWT SecretKey not configured");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.Name!)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(72),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

