namespace TrekTracker.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Relations
    public Route Route { get; set; } = null!;
    public User User { get; set; } = null!;
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    public ICollection<CommentLike> Likes { get; set; } = new List<CommentLike>();
}
