namespace _2026_campus_room_booking_backend.DTOs;

public class RoomScheduleDto
{
    public int BookingId { get; set; }
    public string RoomName { get; set; } = string.Empty;
    public string BookedBy { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public int Status { get; set; }
}
