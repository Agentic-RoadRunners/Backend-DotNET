using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Queries.GetActiveJourney;

public class GetActiveJourneyQuery : IRequest<ApiResponse<UserJourneyResponse?>>
{
    public Guid UserId { get; set; }
}