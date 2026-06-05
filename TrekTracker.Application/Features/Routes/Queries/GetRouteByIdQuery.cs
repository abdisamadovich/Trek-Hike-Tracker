using MediatR;
using TrekTracker.Application.Features.Routes.Dtos;

namespace TrekTracker.Application.Features.Routes.Queries;

public record GetRouteByIdQuery(int RouteId) : IRequest<RouteResponseDto>;
