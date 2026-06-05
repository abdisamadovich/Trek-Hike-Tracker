using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class BookmarkRouteCommandHandler : IRequestHandler<BookmarkRouteCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public BookmarkRouteCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(BookmarkRouteCommand request, CancellationToken cancellationToken)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.RouteId && r.IsActive, cancellationToken);

        if (route == null)
            return false;

        var existingBookmark = await _context.RouteBookmarks
            .FirstOrDefaultAsync(b => b.RouteId == request.RouteId && b.UserId == request.UserId, cancellationToken);

        if (existingBookmark != null)
            return false;

        _context.RouteBookmarks.Add(new RouteBookmark
        {
            RouteId = request.RouteId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
