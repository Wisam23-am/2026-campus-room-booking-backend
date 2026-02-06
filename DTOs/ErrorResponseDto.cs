namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// Standardized error response format for API errors
/// </summary>
public class ErrorResponseDto
{
    /// <summary>
    /// HTTP status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Detailed validation errors (field name -> error messages)
    /// </summary>
    public Dictionary<string, List<string>>? Errors { get; set; }
}
