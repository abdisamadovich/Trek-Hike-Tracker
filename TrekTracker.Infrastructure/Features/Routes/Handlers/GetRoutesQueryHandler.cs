using MediatR;
using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Dtos;
using TrekTracker.Application.Features.Routes.Dtos;
using TrekTracker.Application.Features.Routes.Queries;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Features.Routes.Handlers;

public class GetRoutesQueryHandler : IRequestHandler<GetRoutesQuery, PagedResponseDto<RouteResponseDto>>
{
    private readonly TrekTrackerDbContext _context;

    public GetRoutesQueryHandler(TrekTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponseDto<RouteResponseDto>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Routes.Where(r => r.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Filter.SearchQuery))
        {
            var searchTerm = request.Filter.SearchQuery.ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(searchTerm) ||
                                      r.Description.ToLower().Contains(searchTerm));
        }

        if (request.Filter.Difficulty.HasValue)
            query = query.Where(r => r.Difficulty == request.Filter.Difficulty);

        if (request.Filter.MinDistance.HasValue)
            query = query.Where(r => r.DistanceKm >= request.Filter.MinDistance);

        if (request.Filter.MaxDistance.HasValue)
            query = query.Where(r => r.DistanceKm <= request.Filter.MaxDistance);

        if (request.Filter.Season.HasValue)
            query = query.Where(r => r.Season == request.Filter.Season);

        if (!string.IsNullOrWhiteSpace(request.Filter.Region))
            query = query.Where(r => r.Region == request.Filter.Region);

        query = request.Filter.SortBy switch
        {
            "popular" => query.OrderByDescending(r => r.Likes.Count),
            "rating" => query.OrderByDescending(r => r.Ratings.Average(rt => (double?)rt.Value) ?? 0),
            _ => query.OrderByDescending(r => r.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var routes = await query
            .Skip((request.Filter.Page - 1) * request.Filter.PageSize)
            .Take(request.Filter.PageSize)
            .ToListAsync(cancellationToken);

        var responseItems = new List<RouteResponseDto>();
        foreach (var route in routes)
        {
            responseItems.Add(await MapToResponseDto(route, cancellationToken));
        }

        return new PagedResponseDto<RouteResponseDto>
        {
            Items = responseItems,
            TotalCount = totalCount,
            Page = request.Filter.Page,
            PageSize = request.Filter.PageSize
        };
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
