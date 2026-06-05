namespace TrekTracker.Application.Features.Social.Dtos;

public class CreateCommentRequestDto
{
    public string Text { get; set; } = string.Empty;
    public int? ParentCommentId { get; set; }
}
