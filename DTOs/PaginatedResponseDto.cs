namespace _2026_campus_room_booking_backend.DTOs;

/// <summary>
/// Paginated response wrapper with metadata
/// </summary>
public class PaginatedResponseDto<T>
{
    /// <summary>
    /// List of items for the current page
    /// </summary>
    public List<T> Data { get; set; } = new();

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Whether there is a previous page
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    /// Whether there is a next page
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;
}
