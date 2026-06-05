using MediatR;

namespace TrekTracker.Application.Features.Social.Commands;

public record DeleteCommentCommand(int CommentId, int UserId) : IRequest<bool>;
