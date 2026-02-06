using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for room booking response
/// </summary>
public class BookingResponseDto
{
    /// <summary>
    /// Unique identifier for the booking
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the room being booked
    /// </summary>
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// Name of the person booking the room
    /// </summary>
    public string BookedBy { get; set; } = string.Empty;

    /// <summary>
    /// Purpose or reason for booking the room
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// Start date and time of the booking
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End date and time of the booking
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Current status of the booking
    /// </summary>
    public BookingStatus Status { get; set; }

    /// <summary>
    /// Date and time when the booking was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date and time when the booking was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
