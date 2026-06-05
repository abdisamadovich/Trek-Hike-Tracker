using MediatR;
using TrekTracker.Application.Features.Auth.Commands;
using TrekTracker.Application.Features.Auth.Dtos;
using TrekTracker.Application.Interfaces;

namespace TrekTracker.Infrastructure.Features.Auth.Handlers;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request.Request, cancellationToken);
    }
}
