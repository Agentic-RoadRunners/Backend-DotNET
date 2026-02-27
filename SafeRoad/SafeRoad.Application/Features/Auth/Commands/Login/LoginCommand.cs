// Command just contains what is the request.
using MediatR;
using SafeRoad.Core.DTOs.Auth;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<ApiResponse<AuthResponse>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}