
using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ApiResponse<UserProfileResponse>>
{
    private readonly IUserRepository _userRepository;

    public UpdateProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<UserProfileResponse>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithRolesAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        if (request.FullName != null) user.FullName = request.FullName;
        if (request.AvatarUrl != null) user.AvatarUrl = request.AvatarUrl;

        await _userRepository.UpdateAsync(user);

        var response = new UserProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            TrustScore = user.TrustScore,
            Status = user.Status.ToString(),
            Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new(),
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<UserProfileResponse>.Success(response, "Profile updated successfully.");
    }
}