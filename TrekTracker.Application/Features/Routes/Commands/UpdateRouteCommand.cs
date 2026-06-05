using MediatR;
using TrekTracker.Application.Features.Routes.Dtos;

namespace TrekTracker.Application.Features.Routes.Commands;

public record UpdateRouteCommand(int UserId, UpdateRouteRequestDto Request) : IRequest<RouteResponseDto>;
