using System.ComponentModel.DataAnnotations;

namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO for registering a new user.
/// Role is always decided by the server.
/// </summary>
public class AuthRegisterDto
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
