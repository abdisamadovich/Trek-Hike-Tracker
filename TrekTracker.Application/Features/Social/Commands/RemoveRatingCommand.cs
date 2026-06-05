using MediatR;

namespace TrekTracker.Application.Features.Social.Commands;

public record RemoveRatingCommand(int RouteId, int UserId) : IRequest<bool>;
