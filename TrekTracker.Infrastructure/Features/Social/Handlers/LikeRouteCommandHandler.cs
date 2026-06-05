using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class LikeRouteCommandHandler : IRequestHandler<LikeRouteCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public LikeRouteCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(LikeRouteCommand request, CancellationToken cancellationToken)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.RouteId && r.IsActive, cancellationToken);

        if (route == null)
            return false;

        var existingLike = await _context.RouteLikes
            .FirstOrDefaultAsync(l => l.RouteId == request.RouteId && l.UserId == request.UserId, cancellationToken);

        if (existingLike != null)
            return false;

        _context.RouteLikes.Add(new RouteLike
        {
            RouteId = request.RouteId,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
