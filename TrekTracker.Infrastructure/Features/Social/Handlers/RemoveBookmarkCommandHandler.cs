using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class RemoveBookmarkCommandHandler : IRequestHandler<RemoveBookmarkCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public RemoveBookmarkCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveBookmarkCommand request, CancellationToken cancellationToken)
    {
        var bookmark = await _context.RouteBookmarks
            .FirstOrDefaultAsync(b => b.RouteId == request.RouteId && b.UserId == request.UserId, cancellationToken);

        if (bookmark == null)
            return false;

        _context.RouteBookmarks.Remove(bookmark);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
