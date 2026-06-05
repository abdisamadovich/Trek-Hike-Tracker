using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Social.Handlers;

public class RemoveRatingCommandHandler : IRequestHandler<RemoveRatingCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public RemoveRatingCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await _context.RouteRatings
            .FirstOrDefaultAsync(r => r.RouteId == request.RouteId && r.UserId == request.UserId, cancellationToken);

        if (rating == null)
            return false;

        _context.RouteRatings.Remove(rating);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
