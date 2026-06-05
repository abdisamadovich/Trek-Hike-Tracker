using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrekTracker.Application.Features.Social.Commands;
using TrekTracker.Application.Features.Social.Dtos;
using TrekTracker.Application.Features.Social.Queries;

namespace TrekTracker.API.Controllers;

[ApiController]
[Route("api/routes/{routeId}/[controller]")]
public class SocialController : ControllerBase
{
    private readonly IMediator _mediator;

    public SocialController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    }

    // Like endpoints
    [Authorize]
    [HttpPost("like")]
    public async Task<ActionResult<bool>> LikeRoute(int routeId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new LikeRouteCommand(routeId, userId);
        var result = await _mediator.Send(command);

        return result ? Ok(true) : BadRequest("Failed to like route");
    }

    [Authorize]
    [HttpDelete("like")]
    public async Task<ActionResult<bool>> UnlikeRoute(int routeId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new UnlikeRouteCommand(routeId, userId);
        var result = await _mediator.Send(command);

        return result ? Ok(true) : BadRequest("Failed to unlike route");
    }

    // Rating endpoints
    [Authorize]
    [HttpPost("rating")]
    public async Task<ActionResult<bool>> RateRoute(int routeId, [FromBody] RateRouteRequestDto request)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new RateRouteCommand(routeId, userId, request);
        var result = await _mediator.Send(command);

        return result ? Ok(true) : BadRequest("Failed to rate route");
    }

    [Authorize]
    [HttpDelete("rating")]
    public async Task<ActionResult<bool>> RemoveRating(int routeId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new RemoveRatingCommand(routeId, userId);
        var result = await _mediator.Send(command);

        return result ? Ok(true) : BadRequest("Failed to remove rating");
    }

    // Bookmark endpoints
    [Authorize]
    [HttpPost("bookmark")]
    public async Task<ActionResult<bool>> BookmarkRoute(int routeId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new BookmarkRouteCommand(routeId, userId);
        var result = await _mediator.Send(command);

        return result ? Ok(true) : BadRequest("Failed to bookmark route");
    }

    [Authorize]
    [HttpDelete("bookmark")]
    public async Task<ActionResult<bool>> RemoveBookmark(int routeId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new RemoveBookmarkCommand(routeId, userId);
        var result = await _mediator.Send(command);

        return result ? Ok(true) : BadRequest("Failed to remove bookmark");
    }

    // Comment endpoints
    [HttpGet("comments")]
    public async Task<ActionResult<List<CommentResponseDto>>> GetComments(
        int routeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetRouteCommentsQuery(routeId, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("comments")]
    public async Task<ActionResult<CommentResponseDto>> CreateComment(
        int routeId,
        [FromBody] CreateCommentRequestDto request)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new CreateCommentCommand(routeId, userId, request);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetComments), new { routeId }, result);
    }

    [Authorize]
    [HttpPut("comments/{commentId}")]
    public async Task<ActionResult<CommentResponseDto>> UpdateComment(
        int routeId,
        int commentId,
        [FromBody] CreateCommentRequestDto request)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new UpdateCommentCommand(commentId, userId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("comments/{commentId}")]
    public async Task<ActionResult<bool>> DeleteComment(int routeId, int commentId)
    {
        var userId = GetUserId();
        if (userId == 0)
            return Unauthorized();

        var command = new DeleteCommentCommand(commentId, userId);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
}
