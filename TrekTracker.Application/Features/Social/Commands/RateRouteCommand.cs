using MediatR;
using TrekTracker.Application.Features.Social.Dtos;

namespace TrekTracker.Application.Features.Social.Commands;

public record RateRouteCommand(int RouteId, int UserId, RateRouteRequestDto Request) : IRequest<bool>;
