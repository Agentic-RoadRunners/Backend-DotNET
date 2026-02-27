using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Commands.StartJourney;

public class StartJourneyCommand : IRequest<ApiResponse<StartJourneyResponse>>
{
    public Guid UserId { get; set; }
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }
}