
using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}