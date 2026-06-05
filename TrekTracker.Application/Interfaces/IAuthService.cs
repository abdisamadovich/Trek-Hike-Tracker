using TrekTracker.Application.Features.Auth.Dtos;

namespace TrekTracker.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default);
    Task<(bool IsValid, int UserId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
}
