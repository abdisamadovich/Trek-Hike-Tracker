namespace TrekTracker.Domain.Entities;

public class RouteBookmark
{
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relations
    public User User { get; set; } = null!;
    public Route Route { get; set; } = null!;
}
