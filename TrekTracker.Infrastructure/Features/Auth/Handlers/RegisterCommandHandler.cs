using MediatR;
using TrekTracker.Application.Features.Auth.Commands;
using TrekTracker.Application.Features.Auth.Dtos;
using TrekTracker.Application.Interfaces;

namespace TrekTracker.Infrastructure.Features.Auth.Handlers;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(request.Request, cancellationToken);
    }
}
