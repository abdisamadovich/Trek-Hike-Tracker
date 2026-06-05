using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrekTracker.Application.Features.Auth.Commands;
using TrekTracker.Application.Features.Auth.Dtos;

namespace TrekTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterRequestDto request)
    {
        var command = new RegisterCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
