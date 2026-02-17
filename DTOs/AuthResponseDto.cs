namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// DTO returned after successful authentication.
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public UserResponseDto User { get; set; } = new();
}
