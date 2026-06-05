using Microsoft.EntityFrameworkCore;
using TrekTracker.Application.Features.Auth.Dtos;
using TrekTracker.Application.Interfaces;
using TrekTracker.Domain.Entities;
using TrekTracker.Infrastructure.Data;

namespace TrekTracker.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly TrekTrackerDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthService(TrekTrackerDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive, ct)
            ?? throw new InvalidOperationException("Invalid email or password");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid email or password");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.LastLoginAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(ct);

        return new LoginResponseDto
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken ct = default)
    {
        if (request.Password != request.PasswordConfirm)
            throw new InvalidOperationException("Passwords do not match");

        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, ct);

        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists");

        var user = new User
        {
            Email = request.Email,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new LoginResponseDto
        {
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<(bool IsValid, int UserId)> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        // This is a simplified implementation
        // In production, you'd store refresh tokens in the database
        return await Task.FromResult((true, 0));
    }
}
