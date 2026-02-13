using System.ComponentModel.DataAnnotations;
using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for updating a room
/// </summary>
public class UpdateRoomDto
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Building { get; set; } = string.Empty;

    [Range(0, 200)]
    public int Floor { get; set; }

    [Range(1, 10000)]
    public int Capacity { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Category { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public RoomStatus Status { get; set; }
}
