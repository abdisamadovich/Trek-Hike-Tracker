using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Routes.Commands;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Routes.Handlers;

public class DeleteRouteCommandHandler : IRequestHandler<DeleteRouteCommand, bool>
{
    private readonly TrekTrackerDbContext _context;

    public DeleteRouteCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.RouteId && r.UserId == request.UserId, cancellationToken);

        if (route == null)
            return false;

        _context.Routes.Remove(route);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
