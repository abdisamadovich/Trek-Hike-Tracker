namespace TrekTracker.Domain.Entities;

public class RoutePhoto
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public bool IsCover { get; set; }
    public DateTime UploadedAt { get; set; }

    // Relations
    public Route Route { get; set; } = null!;
}
