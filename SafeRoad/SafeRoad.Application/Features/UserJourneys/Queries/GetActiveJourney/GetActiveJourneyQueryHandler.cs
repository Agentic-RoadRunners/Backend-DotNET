using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Queries.GetActiveJourney;

public class GetActiveJourneyQueryHandler : IRequestHandler<GetActiveJourneyQuery, ApiResponse<UserJourneyResponse?>>
{
    private readonly IUserJourneyRepository _journeyRepository;

    public GetActiveJourneyQueryHandler(IUserJourneyRepository journeyRepository)
    {
        _journeyRepository = journeyRepository;
    }

    public async Task<ApiResponse<UserJourneyResponse?>> Handle(GetActiveJourneyQuery request, CancellationToken cancellationToken)
    {
        var journey = await _journeyRepository.GetActiveByUserIdAsync(request.UserId);
        if (journey == null)
            return ApiResponse<UserJourneyResponse?>.Success(null, "No active journey.");

        var response = new UserJourneyResponse
        {
            Id = journey.Id,
            UserId = journey.UserId,
            StartLatitude = journey.RoutePath.StartPoint.Y,
            StartLongitude = journey.RoutePath.StartPoint.X,
            EndLatitude = journey.RoutePath.EndPoint.Y,
            EndLongitude = journey.RoutePath.EndPoint.X,
            Status = journey.Status.ToString(),
            StartedAt = journey.CreatedAt
        };

        return ApiResponse<UserJourneyResponse?>.Success(response);
    }
}