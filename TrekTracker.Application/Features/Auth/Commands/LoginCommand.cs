using MediatR;
using TrekTracker.Application.Features.Auth.Dtos;

namespace TrekTracker.Application.Features.Auth.Commands;

public record LoginCommand(LoginRequestDto Request) : IRequest<LoginResponseDto>;
