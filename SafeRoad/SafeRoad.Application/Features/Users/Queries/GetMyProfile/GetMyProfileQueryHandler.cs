// Features/Users/Queries/GetMyProfile/GetMyProfileQueryHandler.cs

using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetMyProfile;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, ApiResponse<UserProfileResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetMyProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<UserProfileResponse>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithRolesAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

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

        return ApiResponse<UserProfileResponse>.Success(response);
    }
}