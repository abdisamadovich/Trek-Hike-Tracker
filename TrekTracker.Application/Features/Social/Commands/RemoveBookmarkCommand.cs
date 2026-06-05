using MediatR;

namespace TrekTracker.Application.Features.Social.Commands;

public record RemoveBookmarkCommand(int RouteId, int UserId) : IRequest<bool>;
