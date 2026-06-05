using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class RateRouteCommandHandler : IRequestHandler<RateRouteCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public RateRouteCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RateRouteCommand request, CancellationToken cancellationToken)
    {
        if (request.Request.Value < 1 || request.Request.Value > 5)
            return false;

        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.RouteId && r.IsActive, cancellationToken);

        if (route == null)
            return false;

        var existingRating = await _context.RouteRatings
            .FirstOrDefaultAsync(r => r.RouteId == request.RouteId && r.UserId == request.UserId, cancellationToken);

        if (existingRating != null)
        {
            existingRating.Value = request.Request.Value;
            _context.RouteRatings.Update(existingRating);
        }
        else
        {
            _context.RouteRatings.Add(new RouteRating
            {
                RouteId = request.RouteId,
                UserId = request.UserId,
                Value = request.Request.Value,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
