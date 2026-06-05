using MediatR;
using TrekTracker.Application.Features.Social.Dtos;

namespace TrekTracker.Application.Features.Social.Commands;

public record UpdateCommentCommand(int CommentId, int UserId, CreateCommentRequestDto Request) : IRequest<CommentResponseDto>;
