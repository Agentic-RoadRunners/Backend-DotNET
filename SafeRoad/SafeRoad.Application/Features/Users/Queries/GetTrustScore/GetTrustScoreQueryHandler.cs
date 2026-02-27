
using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetTrustScore;

public class GetTrustScoreQueryHandler : IRequestHandler<GetTrustScoreQuery, ApiResponse<int>>
{
    private readonly IUserRepository _userRepository;

    public GetTrustScoreQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<int>> Handle(GetTrustScoreQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        return ApiResponse<int>.Success(user.TrustScore);
    }
}