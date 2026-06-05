namespace TrekTracker.Domain.Entities;

public class RouteTag
{
    public int RouteId { get; set; }
    public int TagId { get; set; }

    // Relations
    public Route Route { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
