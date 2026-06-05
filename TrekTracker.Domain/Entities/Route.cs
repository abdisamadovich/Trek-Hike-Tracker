namespace TrekTracker.Domain.Entities;

using TrekTracker.Domain.Enums;

public class Route
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Location
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }

    // Route details
    public DifficultyLevel Difficulty { get; set; }
    public double DistanceKm { get; set; }
    public int ElevationGainM { get; set; }
    public int EstimatedHours { get; set; }
    public Season Season { get; set; }
    public string? Region { get; set; }

    // Metadata
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Relations
    public User User { get; set; } = null!;
    public ICollection<RoutePhoto> Photos { get; set; } = new List<RoutePhoto>();
    public ICollection<RouteTag> RouteTags { get; set; } = new List<RouteTag>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<RouteLike> Likes { get; set; } = new List<RouteLike>();
    public ICollection<RouteRating> Ratings { get; set; } = new List<RouteRating>();
    public ICollection<RouteBookmark> Bookmarks { get; set; } = new List<RouteBookmark>();
}
