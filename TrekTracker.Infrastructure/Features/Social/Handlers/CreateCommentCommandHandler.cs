using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Application.Features.Social.Dtos;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentResponseDto>
{
    private readonly TrekTrackerDbContext _context;

    public CreateCommentCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<CommentResponseDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.RouteId && r.IsActive, cancellationToken);

        if (route == null)
            throw new InvalidOperationException("Route not found");

        var comment = new Comment
        {
            RouteId = request.RouteId,
            UserId = request.UserId,
            ParentId = request.Request.ParentCommentId,
            Text = request.Request.Text,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return await MapToResponseDto(comment, cancellationToken);
    }

    private async Task<CommentResponseDto> MapToResponseDto(Comment comment, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstAsync(u => u.Id == comment.UserId, cancellationToken);
        var likesCount = await _context.CommentLikes.CountAsync(l => l.CommentId == comment.Id, cancellationToken);

        var replies = await _context.Comments
            .Where(c => c.ParentId == comment.Id)
            .ToListAsync(cancellationToken);

        var repliesDto = new List<CommentResponseDto>();
        foreach (var reply in replies)
        {
            repliesDto.Add(await MapToResponseDto(reply, cancellationToken));
        }

        return new CommentResponseDto
        {
            Id = comment.Id,
            RouteId = comment.RouteId,
            UserId = comment.UserId,
            UserName = user.Username,
            UserAvatar = user.AvatarUrl ?? string.Empty,
            Text = comment.Text,
            ParentId = comment.ParentId,
            LikesCount = likesCount,
            Replies = repliesDto,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt
        };
    }
}
