namespace TrekTracker.Application.Features.Social.Dtos;

public class CommentResponseDto
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int LikesCount { get; set; }
    public List<CommentResponseDto> Replies { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
