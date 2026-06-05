using MediatR;

namespace TrekTracker.Application.Features.Social.Commands;

public record UnlikeRouteCommand(int RouteId, int UserId) : IRequest<bool>;
