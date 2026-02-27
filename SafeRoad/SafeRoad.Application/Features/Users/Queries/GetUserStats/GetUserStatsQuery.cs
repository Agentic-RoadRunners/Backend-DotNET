
using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetUserStats;

public class GetUserStatsQuery : IRequest<ApiResponse<UserStatsResponse>>
{
    public Guid UserId { get; set; }
}