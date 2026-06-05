using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Dtos;
using TrekTracker.Application.Features.Social.Queries;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class GetRouteCommentsQueryHandler : IRequestHandler<GetRouteCommentsQuery, List<CommentResponseDto>>
{
    private readonly TrekTrackerDbContext _context;

    public GetRouteCommentsQueryHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<List<CommentResponseDto>> Handle(GetRouteCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _context.Comments
            .Where(c => c.RouteId == request.RouteId && c.ParentId == null)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var result = new List<CommentResponseDto>();
        foreach (var comment in comments)
        {
            result.Add(await MapToResponseDto(comment, cancellationToken));
        }

        return result;
    }

    private async Task<CommentResponseDto> MapToResponseDto(Comment comment, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstAsync(u => u.Id == comment.UserId, cancellationToken);
        var likesCount = await _context.CommentLikes.CountAsync(l => l.CommentId == comment.Id, cancellationToken);

        var replies = await _context.Comments
            .Where(c => c.ParentId == comment.Id)
            .OrderByDescending(c => c.CreatedAt)
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
