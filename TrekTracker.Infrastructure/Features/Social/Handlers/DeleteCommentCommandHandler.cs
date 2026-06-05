using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public DeleteCommentCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == request.CommentId && c.UserId == request.UserId, cancellationToken);

        if (comment == null)
            return false;

        var replies = await _context.Comments
            .Where(c => c.ParentId == comment.Id)
            .ToListAsync(cancellationToken);

        _context.Comments.RemoveRange(replies);
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
