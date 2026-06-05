using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrekTracker.Application.Dtos;
using TrekTracker.Application.Features.Routes.Commands;
using TrekTracker.Application.Features.Routes.Dtos;
using TrekTracker.Application.Features.Routes.Queries;

namespace TrekTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoutesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<RouteResponseDto>>> GetRoutes(
        [FromQuery] string? search,
        [FromQuery] int? difficulty,
        [FromQuery] double? minDistance,
        [FromQuery] double? maxDistance,
        [FromQuery] int? season,
        [FromQuery] string? region,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "newest")
    {
        var filter = new RouteFilterDto
        {
            SearchQuery = search,
            Difficulty = difficulty.HasValue ? (Domain.Enums.DifficultyLevel)difficulty : null,
            MinDistance = minDistance,
            MaxDistance = maxDistance,
            Season = season.HasValue ? (Domain.Enums.Season)season : null,
            Region = region,
            Page = page,
            PageSize = pageSize,
            SortBy = sortBy
        };

        var query = new GetRoutesQuery(filter);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RouteResponseDto>> GetRoute(int id)
    {
        var query = new GetRouteByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RouteResponseDto>> CreateRoute([FromBody] CreateRouteRequestDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (userId == 0)
            return Unauthorized();

        var command = new CreateRouteCommand(userId, request);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRoute), new { id = result.Id }, result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<RouteResponseDto>> UpdateRoute(int id, [FromBody] UpdateRouteRequestDto request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (userId == 0)
            return Unauthorized();

        request.Id = id;
        var command = new UpdateRouteCommand(userId, request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteRoute(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (userId == 0)
            return Unauthorized();

        var command = new DeleteRouteCommand(id, userId);
        var result = await _mediator.Send(command);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
