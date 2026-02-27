using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Queries.GetMyJourneys;

public class GetMyJourneysQuery : IRequest<ApiResponse<List<UserJourneyResponse>>>
{
    public Guid UserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}