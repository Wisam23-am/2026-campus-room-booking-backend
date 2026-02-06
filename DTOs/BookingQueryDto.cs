using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for query parameters (search, filter, sort, pagination)
/// </summary>
public class BookingQueryDto
{
    /// <summary>
    /// Search by RoomName or BookedBy
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Filter by booking status (0=Pending, 1=Approved, 2=Rejected)
    /// </summary>
    public BookingStatus? Status { get; set; }

    /// <summary>
    /// Filter bookings starting from this date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Filter bookings until this date
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Sort by field: CreatedAt, StartTime, RoomName, BookedBy (default: CreatedAt)
    /// </summary>
    public string SortBy { get; set; } = "CreatedAt";

    /// <summary>
    /// Sort order: asc or desc (default: desc)
    /// </summary>
    public string SortOrder { get; set; } = "desc";

    /// <summary>
    /// Page number (default: 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page (default: 10, max: 50)
    /// </summary>
    public int PageSize { get; set; } = 10;
}
