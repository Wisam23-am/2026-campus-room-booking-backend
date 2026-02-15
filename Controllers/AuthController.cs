using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using _2026_campus_room_booking_backend.Data;
using _2026_campus_room_booking_backend.DTOs;
using _2026_campus_room_booking_backend.Enums;
using _2026_campus_room_booking_backend.Models;

namespace _2026_campus_room_booking_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserResponseDto>> Register(AuthRegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(BuildValidationError());
        }

        var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
        var emailExists = await _context.Users.AnyAsync(u => !u.IsDeleted && u.Email.ToLower() == normalizedEmail);
        if (emailExists)
        {
            return Conflict(new ErrorResponseDto
            {
                StatusCode = 409,
                Message = "Validation failed",
                Errors = new Dictionary<string, List<string>>
                {
                    { "Email", new List<string> { "Email is already in use" } }
                }
            });
        }

        var user = new AppUser
        {
            FullName = dto.FullName.Trim(),
            Email = normalizedEmail,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Register), new { id = user.Id }, MapToResponseDto(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(AuthLoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(BuildValidationError());
        }

        var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
        var user = await _context.Users.FirstOrDefaultAsync(u => !u.IsDeleted && u.Email.ToLower() == normalizedEmail);
        if (user == null)
        {
            return Unauthorized(new ErrorResponseDto { StatusCode = 401, Message = "Invalid email or password" });
        }

        var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!isValidPassword)
        {
            return Unauthorized(new ErrorResponseDto { StatusCode = 401, Message = "Invalid email or password" });
        }

        var token = GenerateJwtToken(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            User = MapToResponseDto(user)
        });
    }

    private string GenerateJwtToken(AppUser user)
    {
        var key = _configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_SECRET") ?? "";
        var issuer = _configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "RoomBookingAPI";
        var audience = _configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "RoomBookingClient";
        var expiresMinutesRaw = _configuration["Jwt:ExpiresMinutes"];
        var expiresMinutes = int.TryParse(expiresMinutesRaw, out var parsed) ? parsed : 120;

        if (string.IsNullOrWhiteSpace(key))
        {
            // Fallback: make it explicit rather than generating an inconsistent key.
            // This should be configured via appsettings or environment variables.
            key = "dev-secret-change-me-please-32-bytes-minimum-123456";
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("role", user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ErrorResponseDto BuildValidationError()
    {
        var errors = ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
            );

        return new ErrorResponseDto { StatusCode = 400, Message = "Validation failed", Errors = errors };
    }

    private static UserResponseDto MapToResponseDto(AppUser user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
