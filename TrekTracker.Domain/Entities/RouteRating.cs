namespace TrekTracker.Domain.Entities;

public class RouteRating
{
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public int Value { get; set; } // 1-5
    public DateTime CreatedAt { get; set; }

    // Relations
    public User User { get; set; } = null!;
    public Route Route { get; set; } = null!;
}
