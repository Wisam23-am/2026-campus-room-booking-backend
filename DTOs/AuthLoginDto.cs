using System.ComponentModel.DataAnnotations;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for login.
/// </summary>
public class AuthLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
