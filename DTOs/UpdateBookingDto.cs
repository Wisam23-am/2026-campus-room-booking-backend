using System.ComponentModel.DataAnnotations;
using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for updating an existing room booking
/// </summary>
public class UpdateBookingDto
{
    /// <summary>
    /// Name of the room to book
    /// </summary>
    [Required(ErrorMessage = "Room name is required")]
    [StringLength(100, ErrorMessage = "Room name cannot exceed 100 characters")]
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// Name of the person making the booking
    /// </summary>
    [Required(ErrorMessage = "Booked by is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string BookedBy { get; set; } = string.Empty;

    /// <summary>
    /// Purpose or reason for the booking
    /// </summary>
    [StringLength(500, ErrorMessage = "Purpose cannot exceed 500 characters")]
    public string? Purpose { get; set; }

    /// <summary>
    /// Start date and time of the booking
    /// </summary>
    [Required(ErrorMessage = "Start time is required")]
    [DataType(DataType.DateTime)]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End date and time of the booking
    /// </summary>
    [Required(ErrorMessage = "End time is required")]
    [DataType(DataType.DateTime)]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Status of the booking
    /// </summary>
    [Required(ErrorMessage = "Status is required")]
    public BookingStatus Status { get; set; }
}
