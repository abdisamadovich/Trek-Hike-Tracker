using TrekTracker.Domain.Enums;

namespace TrekTracker.Application.Features.Routes.Dtos;

public class RouteFilterDto
{
    public string? SearchQuery { get; set; }
    public DifficultyLevel? Difficulty { get; set; }
    public double? MinDistance { get; set; }
    public double? MaxDistance { get; set; }
    public Season? Season { get; set; }
    public string? Region { get; set; }
    public double? MinRating { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "newest"; // newest, popular, rating
}
