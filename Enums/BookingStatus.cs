namespace _2026_campus_room_booking_backend.Enums;

/// <summary>
/// Represents the status of a room booking
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Booking is waiting for approval
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Booking has been approved
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Booking has been rejected
    /// </summary>
    Rejected = 2
}
