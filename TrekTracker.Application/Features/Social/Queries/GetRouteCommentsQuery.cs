using MediatR;
using TrekTracker.Application.Features.Social.Dtos;

namespace TrekTracker.Application.Features.Social.Queries;

public record GetRouteCommentsQuery(int RouteId, int Page = 1, int PageSize = 10) : IRequest<List<CommentResponseDto>>;
