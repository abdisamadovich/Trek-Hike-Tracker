namespace TrekTracker.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Relations
    public ICollection<RouteTag> RouteTags { get; set; } = new List<RouteTag>();
}
