using MediatR;
using TrekTracker.Application.Features.Social.Dtos;

namespace TrekTracker.Application.Features.Social.Commands;

public record CreateCommentCommand(int RouteId, int UserId, CreateCommentRequestDto Request) : IRequest<CommentResponseDto>;
