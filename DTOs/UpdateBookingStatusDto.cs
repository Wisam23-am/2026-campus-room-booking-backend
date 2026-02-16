using System.ComponentModel.DataAnnotations;
using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for updating a booking status only (approve/reject)
/// </summary>
public class UpdateBookingStatusDto
{
    /// <summary>
    /// Status of the booking
    /// </summary>
    [Required(ErrorMessage = "Status is required")]
    public BookingStatus Status { get; set; }
}
