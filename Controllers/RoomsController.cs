using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using _2026_campus_room_booking_backend.Data;
using _2026_campus_room_booking_backend.DTOs;
using _2026_campus_room_booking_backend.Models;

namespace _2026_campus_room_booking_backend.Controllers;

/// <summary>
/// Controller for managing rooms
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<RoomResponseDto>>> GetRooms([FromQuery] RoomQueryDto query)
    {
        var roomsQuery = _context.Rooms.Where(r => !r.IsDeleted).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            roomsQuery = roomsQuery.Where(r =>
                r.Name.ToLower().Contains(search) ||
                r.Building.ToLower().Contains(search) ||
                r.Category.ToLower().Contains(search));
        }

        roomsQuery = query.SortBy?.ToLower() switch
        {
            "name" => query.SortOrder?.ToLower() == "asc" ? roomsQuery.OrderBy(r => r.Name) : roomsQuery.OrderByDescending(r => r.Name),
            "building" => query.SortOrder?.ToLower() == "asc" ? roomsQuery.OrderBy(r => r.Building) : roomsQuery.OrderByDescending(r => r.Building),
            "capacity" => query.SortOrder?.ToLower() == "asc" ? roomsQuery.OrderBy(r => r.Capacity) : roomsQuery.OrderByDescending(r => r.Capacity),
            _ => query.SortOrder?.ToLower() == "asc" ? roomsQuery.OrderBy(r => r.CreatedAt) : roomsQuery.OrderByDescending(r => r.CreatedAt)
        };

        var pageSize = Math.Min(query.PageSize, 50);
        var page = Math.Max(query.Page, 1);
        var totalCount = await roomsQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var rooms = await roomsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => MapToResponseDto(r))
            .ToListAsync();

        return Ok(new PaginatedResponseDto<RoomResponseDto>
        {
            Data = rooms,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomResponseDto>> GetRoom(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        if (room == null)
        {
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"Room with ID {id} not found" });
        }

        return Ok(MapToResponseDto(room));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomResponseDto>> CreateRoom(CreateRoomDto dto)
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

        var room = new Room
        {
            Name = dto.Name,
            Building = dto.Building,
            Floor = dto.Floor,
            Capacity = dto.Capacity,
            Category = dto.Category,
            Description = dto.Description,
            Status = dto.Status,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, MapToResponseDto(room));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RoomResponseDto>> UpdateRoom(int id, UpdateRoomDto dto)
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

        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        if (room == null)
        {
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"Room with ID {id} not found" });
        }

        room.Name = dto.Name;
        room.Building = dto.Building;
        room.Floor = dto.Floor;
        room.Capacity = dto.Capacity;
        room.Category = dto.Category;
        room.Description = dto.Description;
        room.Status = dto.Status;
        room.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(MapToResponseDto(room));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        if (room == null)
        {
            return NotFound(new ErrorResponseDto { StatusCode = 404, Message = $"Room with ID {id} not found" });
        }

        room.IsDeleted = true;
        room.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static RoomResponseDto MapToResponseDto(Room room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            Name = room.Name,
            Building = room.Building,
            Floor = room.Floor,
            Capacity = room.Capacity,
            Category = room.Category,
            Description = room.Description,
            Status = room.Status,
            CreatedAt = room.CreatedAt,
            UpdatedAt = room.UpdatedAt
        };
    }
}
