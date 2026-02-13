using System.ComponentModel.DataAnnotations;
using _2026_campus_room_booking_backend.Enums;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for updating a user
/// </summary>
public class UpdateUserDto
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }
}
