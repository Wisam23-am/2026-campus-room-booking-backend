using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for room response
/// </summary>
public class RoomResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Building { get; set; } = string.Empty;

    public int Floor { get; set; }

    public int Capacity { get; set; }

    public string Category { get; set; } = string.Empty;

    public string? Description { get; set; }

    public RoomStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
