using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Routes.Commands;
using TrekTracker.Application.Features.Routes.Dtos;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Routes.Handlers;

public class UpdateRouteCommandHandler : IRequestHandler<UpdateRouteCommand, RouteResponseDto>
{
    private readonly TrekTrackerDbContext _context;

    public UpdateRouteCommandHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<RouteResponseDto> Handle(UpdateRouteCommand request, CancellationToken cancellationToken)
    {
        var route = await _context.Routes
            .FirstOrDefaultAsync(r => r.Id == request.Request.Id && r.UserId == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Route not found or you don't have permission to update it");

        route.Name = request.Request.Name;
        route.Description = request.Request.Description;
        route.StartLatitude = request.Request.StartLatitude;
        route.StartLongitude = request.Request.StartLongitude;
        route.EndLatitude = request.Request.EndLatitude;
        route.EndLongitude = request.Request.EndLongitude;
        route.Difficulty = request.Request.Difficulty;
        route.DistanceKm = request.Request.DistanceKm;
        route.ElevationGainM = request.Request.ElevationGainM;
        route.EstimatedHours = request.Request.EstimatedHours;
        route.Season = request.Request.Season;
        route.Region = request.Request.Region;
        route.UpdatedAt = DateTime.UtcNow;

        _context.Routes.Update(route);

        var currentTags = await _context.RouteTags
            .Where(rt => rt.RouteId == route.Id)
            .ToListAsync(cancellationToken);

        _context.RouteTags.RemoveRange(currentTags);

        if (request.Request.Tags.Any())
        {
            var existingTags = await _context.Tags
                .Where(t => request.Request.Tags.Contains(t.Name))
                .ToListAsync(cancellationToken);

            foreach (var tagName in request.Request.Tags)
            {
                var tag = existingTags.FirstOrDefault(t => t.Name == tagName)
                    ?? new Tag { Name = tagName };

                if (tag.Id == 0)
                    _context.Tags.Add(tag);

                _context.RouteTags.Add(new RouteTag { RouteId = route.Id, Tag = tag });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

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
