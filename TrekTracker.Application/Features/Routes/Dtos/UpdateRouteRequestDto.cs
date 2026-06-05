using TrekTracker.Domain.Enums;

namespace TrekTracker.Application.Features.Routes.Dtos;

public class UpdateRouteRequestDto
{
    public int Id { get; set; }
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
    public List<string> Tags { get; set; } = new();
}
