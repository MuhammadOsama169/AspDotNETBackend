

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetBackend.Data;
using DotNetBackend.DTO.User;
using DotNetBackend.Features.Commands.Auth;
using DotNetBackend.Features.Commands.Login;
using DotNetBackend.Features.Commands.Register;
using DotNetBackend.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOtpService _otpService;
        public AuthController(
            IOtpService otpService,
            IMediator mediator
            )
        {
            _mediator = mediator;
            _otpService = otpService;
        }


        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
        {
            var cmd = new RegisterUserCommand(dto.Name, dto.Email, dto.Password);
            var result = await _mediator.Send(cmd);

            if (result.UserId == Guid.Empty)
                return Conflict(new { message = "Email already in use." });

            return Ok(result);
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto  dto)
        {
            try
            {
                var result = await _mediator.Send(new LoginUserCommand
                {
                    Email    = dto.Email,
                    Password = dto.Password
                });
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }
        }

        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
        {
            var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(sub, out var userId))
                return Unauthorized();

            // mediator.Send returns Unit, which we ignore
            await _mediator.Send(new LogoutCommand {
                UserId       = userId,
                RefreshToken = dto.RefreshToken
            });

            // now return a friendly message
            return Ok(new { message = "Action successful." });
        }
        
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(Guid userId, string code)
        {
            var ok = await _otpService.ValidateAsync(userId, code);
            if (!ok) return BadRequest("Invalid or expired code");
            return Ok("Email verified");
        }
    }
}

