using System.ComponentModel.DataAnnotations;

namespace _2026_campus_room_booking_backend.DTOs;

public class RoomAvailabilityQueryDto
{
    [Required(ErrorMessage = "Start time is required")]
    public string StartTime { get; set; } = string.Empty;

    [Required(ErrorMessage = "End time is required")]
    public string EndTime { get; set; } = string.Empty;
}

public class RoomAvailabilityResponseDto
{
    public int RoomId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<RoomScheduleDto> ConflictingBookings { get; set; } = new();
}
