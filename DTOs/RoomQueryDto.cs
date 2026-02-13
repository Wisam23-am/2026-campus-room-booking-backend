namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for room query parameters (search, sort, pagination)
/// </summary>
public class RoomQueryDto
{
    public string? Search { get; set; }

    public string SortBy { get; set; } = "CreatedAt";

    public string SortOrder { get; set; } = "desc";

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
