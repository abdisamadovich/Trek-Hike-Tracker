using TrekTracker.Domain.Enums;

namespace TrekTracker.Application.Features.Routes.Dtos;

public class RouteResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public double DistanceKm { get; set; }
    public int ElevationGainM { get; set; }
    public int EstimatedHours { get; set; }
    public Season Season { get; set; }
    public string? Region { get; set; }
    public int LikesCount { get; set; }
    public double AverageRating { get; set; }
    public int CommentsCount { get; set; }
    public List<string> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
