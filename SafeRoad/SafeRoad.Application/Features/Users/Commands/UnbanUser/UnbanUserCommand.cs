using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.UnbanUser;

public class UnbanUserCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
}
