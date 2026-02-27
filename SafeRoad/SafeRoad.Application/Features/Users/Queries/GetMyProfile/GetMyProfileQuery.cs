
using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetMyProfile;

public class GetMyProfileQuery : IRequest<ApiResponse<UserProfileResponse>>
{
    public Guid UserId { get; set; }
}