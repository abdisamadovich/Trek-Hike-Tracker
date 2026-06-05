namespace TrekTracker.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; } = string.Empty; // like, comment, rating
    public string Payload { get; set; } = string.Empty; // JSON
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relations
    public User User { get; set; } = null!;
}
