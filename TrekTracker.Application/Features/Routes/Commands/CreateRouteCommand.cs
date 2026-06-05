using MediatR;
using TrekTracker.Application.Features.Routes.Dtos;

namespace TrekTracker.Application.Features.Routes.Commands;

public record CreateRouteCommand(int UserId, CreateRouteRequestDto Request) : IRequest<RouteResponseDto>;
