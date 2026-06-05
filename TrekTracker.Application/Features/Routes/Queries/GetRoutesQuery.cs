using MediatR;
using TrekTracker.Application.Dtos;
using TrekTracker.Application.Features.Routes.Dtos;

namespace TrekTracker.Application.Features.Routes.Queries;

public record GetRoutesQuery(RouteFilterDto Filter) : IRequest<PagedResponseDto<RouteResponseDto>>;
