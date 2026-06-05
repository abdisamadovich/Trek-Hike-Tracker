namespace TrekTracker.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Relations
    public ICollection<Route> Routes { get; set; } = new List<Route>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<RouteLike> RouteLikes { get; set; } = new List<RouteLike>();
    public ICollection<RouteRating> RouteRatings { get; set; } = new List<RouteRating>();
    public ICollection<RouteBookmark> RouteBookmarks { get; set; } = new List<RouteBookmark>();
    public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
