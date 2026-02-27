        
using MediatR;
using SafeRoad.Core.DTOs.Auth;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<ApiResponse<AuthResponse>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? FullName { get; set; }
}