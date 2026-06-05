namespace TrekTracker.Domain.Entities;

public class CommentLike
{
    public int UserId { get; set; }
    public int CommentId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relations
    public User User { get; set; } = null!;
    public Comment Comment { get; set; } = null!;
}
