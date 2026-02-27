using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Auth.Commands.Logout;

public class LogoutCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
}