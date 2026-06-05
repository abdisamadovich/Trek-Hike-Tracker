using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class UnlikeRouteCommandHandler : IRequestHandler<UnlikeRouteCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public UnlikeRouteCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UnlikeRouteCommand request, CancellationToken cancellationToken)
    {
        var like = await _context.RouteLikes
            .FirstOrDefaultAsync(l => l.RouteId == request.RouteId && l.UserId == request.UserId, cancellationToken);

        if (like == null)
            return false;

        _context.RouteLikes.Remove(like);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
