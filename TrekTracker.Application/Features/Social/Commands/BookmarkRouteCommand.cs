using MediatR;

namespace TrekTracker.Application.Features.Social.Commands;

public record BookmarkRouteCommand(int RouteId, int UserId) : IRequest<bool>;
