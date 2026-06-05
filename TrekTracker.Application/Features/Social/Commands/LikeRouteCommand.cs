using MediatR;

namespace TrekTracker.Application.Features.Social.Commands;

public record LikeRouteCommand(int RouteId, int UserId) : IRequest<bool>;
