
using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<ApiResponse<UserProfileResponse>>
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
}