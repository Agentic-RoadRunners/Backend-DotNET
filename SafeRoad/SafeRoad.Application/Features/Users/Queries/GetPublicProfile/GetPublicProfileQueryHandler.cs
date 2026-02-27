
using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetPublicProfile;

public class GetPublicProfileQueryHandler : IRequestHandler<GetPublicProfileQuery, ApiResponse<PublicProfileResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetPublicProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<PublicProfileResponse>> Handle(GetPublicProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        var response = new PublicProfileResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            TrustScore = user.TrustScore,
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<PublicProfileResponse>.Success(response);
    }
}