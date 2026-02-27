
using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetTrustScore;

public class GetTrustScoreQuery : IRequest<ApiResponse<int>>
{
    public Guid UserId { get; set; }
}