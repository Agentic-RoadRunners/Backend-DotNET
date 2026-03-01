using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.BanUser;

public class BanUserCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
}
