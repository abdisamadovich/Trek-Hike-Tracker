using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Routes.Dtos;
using TrekTracker.Application.Features.Routes.Queries;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Routes.Handlers;

public class GetRouteByIdQueryHandler : IRequestHandler<GetRouteByIdQuery, RouteResponseDto>
{
    private readonly TrekTrackerDbContext _context;

    public GetRouteByIdQueryHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<RouteResponseDto> Handle(GetRouteByIdQuery request, CancellationToken cancellationToken)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.RouteId && r.IsActive, cancellationToken)
            ?? throw new InvalidOperationException("Route not found");

        return await MapToResponseDto(route, cancellationToken);
    }

    private async Task<RouteResponseDto> MapToResponseDto(Route route, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstAsync(u => u.Id == route.UserId, cancellationToken);
        var likesCount = await _context.RouteLikes.CountAsync(l => l.RouteId == route.Id, cancellationToken);
        var ratings = await _context.RouteRatings.Where(r => r.RouteId == route.Id).ToListAsync(cancellationToken);
        var commentsCount = await _context.Comments.CountAsync(c => c.RouteId == route.Id, cancellationToken);
        var tags = await _context.RouteTags
            .Where(rt => rt.RouteId == route.Id)
            .Include(rt => rt.Tag)
            .Select(rt => rt.Tag.Name)
            .ToListAsync(cancellationToken);

        return new RouteResponseDto
        {
            Id = route.Id,
            UserId = route.UserId,
            UserName = user.Username,
            Name = route.Name,
            Description = route.Description,
            StartLatitude = route.StartLatitude,
            StartLongitude = route.StartLongitude,
            EndLatitude = route.EndLatitude,
            EndLongitude = route.EndLongitude,
            Difficulty = route.Difficulty,
            DistanceKm = route.DistanceKm,
            ElevationGainM = route.ElevationGainM,
            EstimatedHours = route.EstimatedHours,
            Season = route.Season,
            Region = route.Region,
            LikesCount = likesCount,
            AverageRating = ratings.Any() ? ratings.Average(r => r.Value) : 0,
            CommentsCount = commentsCount,
            Tags = tags,
            CreatedAt = route.CreatedAt,
            UpdatedAt = route.UpdatedAt
        };
    }
}
