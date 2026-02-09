using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2026_campus_room_booking_backend.Data;
using _2026_campus_room_booking_backend.DTOs;
using _2026_campus_room_booking_backend.Enums;
using _2026_campus_room_booking_backend.Models;

namespace _2026_campus_room_booking_backend.Controllers;

/// <summary>
/// Controller for managing room bookings
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomBookingController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomBookingController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all bookings with search, filter, sort, and pagination
    /// </summary>
    /// <param name="query">Query parameters for filtering, searching, sorting and pagination</param>
    /// <returns>Paginated list of bookings</returns>
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<BookingResponseDto>>> GetBookings([FromQuery] BookingQueryDto query)
    {
        var bookingsQuery = _context.RoomBookings
            .Where(b => !b.IsDeleted)
            .AsQueryable();

        // Search by RoomName or BookedBy
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            bookingsQuery = bookingsQuery.Where(b =>
                b.RoomName.ToLower().Contains(search) ||
                b.BookedBy.ToLower().Contains(search));
        }

        // Filter by status
        if (query.Status.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.Status == query.Status.Value);
        }

        // Filter by date range
        if (query.StartDate.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.StartTime >= query.StartDate.Value);
        }

        if (query.EndDate.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.EndTime <= query.EndDate.Value);
        }

        // Sorting
        bookingsQuery = query.SortBy?.ToLower() switch
        {
            "starttime" => query.SortOrder?.ToLower() == "asc"
                ? bookingsQuery.OrderBy(b => b.StartTime)
                : bookingsQuery.OrderByDescending(b => b.StartTime),
            "roomname" => query.SortOrder?.ToLower() == "asc"
                ? bookingsQuery.OrderBy(b => b.RoomName)
                : bookingsQuery.OrderByDescending(b => b.RoomName),
            "bookedby" => query.SortOrder?.ToLower() == "asc"
                ? bookingsQuery.OrderBy(b => b.BookedBy)
                : bookingsQuery.OrderByDescending(b => b.BookedBy),
            _ => query.SortOrder?.ToLower() == "asc"
                ? bookingsQuery.OrderBy(b => b.CreatedAt)
                : bookingsQuery.OrderByDescending(b => b.CreatedAt)
        };

        // Pagination
        var pageSize = Math.Min(query.PageSize, 50); // Max 50 items per page
        var page = Math.Max(query.Page, 1); // Min page 1
        var totalCount = await bookingsQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var bookings = await bookingsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => MapToResponseDto(b))
            .ToListAsync();

        var response = new PaginatedResponseDto<BookingResponseDto>
        {
            Data = bookings,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Ok(response);
    }

    /// <summary>
    /// Get a booking by ID
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <returns>Booking detail</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponseDto>> GetBooking(int id)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (booking == null)
        {
            return NotFound(new ErrorResponseDto
            {
                StatusCode = 404,
                Message = $"Booking with ID {id} not found"
            });
        }

        return Ok(MapToResponseDto(booking));
    }

    /// <summary>
    /// Create a new booking
    /// </summary>
    /// <param name="dto">Booking data</param>
    /// <returns>Created booking</returns>
    [HttpPost]
    public async Task<ActionResult<BookingResponseDto>> CreateBooking(CreateBookingDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return BadRequest(new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = errors
            });
        }

        // Validate: EndTime must be after StartTime
        if (dto.EndTime <= dto.StartTime)
        {
            return BadRequest(new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = new Dictionary<string, List<string>>
                {
                    { "EndTime", new List<string> { "End time must be after start time" } }
                }
            });
        }

        // Validate: Cannot book in the past
        if (dto.StartTime < DateTime.UtcNow)
        {
            return BadRequest(new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = new Dictionary<string, List<string>>
                {
                    { "StartTime", new List<string> { "Cannot book in the past" } }
                }
            });
        }

        // Validate: No overlapping bookings for the same room
        var hasOverlap = await _context.RoomBookings.AnyAsync(b =>
            !b.IsDeleted &&
            b.RoomName == dto.RoomName &&
            b.StartTime < dto.EndTime &&
            b.EndTime > dto.StartTime);

        if (hasOverlap)
        {
            return Conflict(new ErrorResponseDto
            {
                StatusCode = 409,
                Message = "Validation failed",
                Errors = new Dictionary<string, List<string>>
                {
                    { "Schedule", new List<string> { $"Room '{dto.RoomName}' is already booked during the requested time slot" } }
                }
            });
        }

        var booking = new RoomBooking
        {
            RoomName = dto.RoomName,
            BookedBy = dto.BookedBy,
            Purpose = dto.Purpose,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.RoomBookings.Add(booking);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBooking),
            new { id = booking.Id },
            MapToResponseDto(booking));
    }

    /// <summary>
    /// Update an existing booking
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <param name="dto">Updated booking data</param>
    /// <returns>Updated booking</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<BookingResponseDto>> UpdateBooking(int id, UpdateBookingDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return BadRequest(new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = errors
            });
        }

        // Validate: EndTime must be after StartTime
        if (dto.EndTime <= dto.StartTime)
        {
            return BadRequest(new ErrorResponseDto
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = new Dictionary<string, List<string>>
                {
                    { "EndTime", new List<string> { "End time must be after start time" } }
                }
            });
        }

        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (booking == null)
        {
            return NotFound(new ErrorResponseDto
            {
                StatusCode = 404,
                Message = $"Booking with ID {id} not found"
            });
        }

        // Validate: No overlapping bookings for the same room (exclude current booking)
        var hasOverlap = await _context.RoomBookings.AnyAsync(b =>
            !b.IsDeleted &&
            b.Id != id &&
            b.RoomName == dto.RoomName &&
            b.StartTime < dto.EndTime &&
            b.EndTime > dto.StartTime);

        if (hasOverlap)
        {
            return Conflict(new ErrorResponseDto
            {
                StatusCode = 409,
                Message = "Validation failed",
                Errors = new Dictionary<string, List<string>>
                {
                    { "Schedule", new List<string> { $"Room '{dto.RoomName}' is already booked during the requested time slot" } }
                }
            });
        }

        booking.RoomName = dto.RoomName;
        booking.BookedBy = dto.BookedBy;
        booking.Purpose = dto.Purpose;
        booking.StartTime = dto.StartTime;
        booking.EndTime = dto.EndTime;
        booking.Status = dto.Status;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(MapToResponseDto(booking));
    }

    /// <summary>
    /// Soft delete a booking
    /// </summary>
    /// <param name="id">Booking ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _context.RoomBookings
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (booking == null)
        {
            return NotFound(new ErrorResponseDto
            {
                StatusCode = 404,
                Message = $"Booking with ID {id} not found"
            });
        }

        booking.IsDeleted = true;
        booking.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Maps a RoomBooking entity to BookingResponseDto
    /// </summary>
    private static BookingResponseDto MapToResponseDto(RoomBooking booking)
    {
        return new BookingResponseDto
        {
            Id = booking.Id,
            RoomName = booking.RoomName,
            BookedBy = booking.BookedBy,
            Purpose = booking.Purpose,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }
}
