using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;
using _2026_campus_room_booking_backend.Data;
using _2026_campus_room_booking_backend.DTOs;
using _2026_campus_room_booking_backend.Models;

namespace _2026_campus_room_booking_backend.Controllers;

/// <summary>
/// Controller for managing users (minimal, without authentication)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<UserResponseDto>>> GetUsers([FromQuery] UserQueryDto query)
    {
        var usersQuery = _context.Users.Where(u => !u.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            usersQuery = usersQuery.Where(u =>
                u.FullName.ToLower().Contains(search) ||
                u.Email.ToLower().Contains(search));
        }

        usersQuery = query.SortBy?.ToLower() switch
        {
            "fullname" => query.SortOrder?.ToLower() == "asc" ? usersQuery.OrderBy(u => u.FullName) : usersQuery.OrderByDescending(u => u.FullName),
            "email" => query.SortOrder?.ToLower() == "asc" ? usersQuery.OrderBy(u => u.Email) : usersQuery.OrderByDescending(u => u.Email),
            _ => query.SortOrder?.ToLower() == "asc" ? usersQuery.OrderBy(u => u.CreatedAt) : usersQuery.OrderByDescending(u => u.CreatedAt)
        };

        var pageSize = Math.Min(query.PageSize, 50);
        var page = Math.Max(query.Page, 1);
        var totalCount = await usersQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var users = await usersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => MapToResponseDto(u))
            .ToListAsync();

        return Ok(new PaginatedResponseDto<UserResponseDto>
        {
            Data = users,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null)
        {
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"User with ID {id} not found" });
        }

        return Ok(MapToResponseDto(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return BadRequest(new ErrorResponseDto { StatusCode = 400, Message = "Validation failed", Errors = errors });
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
            FullName = dto.FullName,
            Email = normalizedEmail,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, MapToResponseDto(user));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> UpdateUser(int id, UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return BadRequest(new ErrorResponseDto { StatusCode = 400, Message = "Validation failed", Errors = errors });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null)
        {
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"User with ID {id} not found" });
        }

        var emailExists = await _context.Users.AnyAsync(u => !u.IsDeleted && u.Id != id && u.Email.ToLower() == dto.Email.ToLower());
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

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Role = dto.Role;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToResponseDto(user));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null)
        {
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"User with ID {id} not found" });
        }

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
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
