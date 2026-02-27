using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Commands.EndJourney;

public class EndJourneyCommand : IRequest<ApiResponse<EndJourneyResponse>>
{
    public Guid UserId { get; set; }
}