using System.ComponentModel.DataAnnotations;
using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.Models;

/// <summary>
/// Represents a room booking entity
/// </summary>
public class RoomBooking
{
    /// <summary>
    /// Unique identifier for the booking
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the room being booked
    /// </summary>
    [Required]
    [StringLength(100)]
    public string RoomName { get; set; } = string.Empty;

    /// <summary>
    /// Name of the person booking the room
    /// </summary>
    [Required]
    [StringLength(100)]
    public string BookedBy { get; set; } = string.Empty;

    /// <summary>
    /// Purpose or reason for booking the room
    /// </summary>
    [StringLength(500)]
    public string? Purpose { get; set; }

    /// <summary>
    /// Start date and time of the booking
    /// </summary>
    [Required]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// End date and time of the booking
    /// </summary>
    [Required]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Current status of the booking
    /// </summary>
    [Required]
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// Date and time when the booking was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the booking was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Soft delete flag
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}
