using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Commands.EndJourney;

public class EndJourneyCommandHandler : IRequestHandler<EndJourneyCommand, ApiResponse<EndJourneyResponse>>
{
    private readonly IUserJourneyRepository _journeyRepository;

    public EndJourneyCommandHandler(IUserJourneyRepository journeyRepository)
    {
        _journeyRepository = journeyRepository;
    }

    public async Task<ApiResponse<EndJourneyResponse>> Handle(EndJourneyCommand request, CancellationToken cancellationToken)
    {
        var journey = await _journeyRepository.GetActiveByUserIdAsync(request.UserId);
        if (journey == null)
            throw new NotFoundException("Active journey", request.UserId);

        journey.Status = JourneyStatus.Completed;
        await _journeyRepository.UpdateAsync(journey);

        var distanceKm = Math.Round(journey.RoutePath.Length * 111.32, 1);
        var durationMinutes = (int)(DateTime.UtcNow - journey.CreatedAt).TotalMinutes;

        var response = new EndJourneyResponse
        {
            JourneyId = journey.Id,
            DistanceInKm = distanceKm,
            DurationMinutes = durationMinutes,
            Message = "Journey completed. Thank you for using SafeRoad!",
            AskForIncidentReport = true,
            IncidentPrompt = "Did you notice any road incidents during your trip? Help others by reporting them!"
        };

        return ApiResponse<EndJourneyResponse>.Success(response, "Journey ended successfully.");
    }
}