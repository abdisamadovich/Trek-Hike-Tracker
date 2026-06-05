using MediatR;

namespace TrekTracker.Application.Features.Routes.Commands;

public record DeleteRouteCommand(int RouteId, int UserId) : IRequest<bool>;
