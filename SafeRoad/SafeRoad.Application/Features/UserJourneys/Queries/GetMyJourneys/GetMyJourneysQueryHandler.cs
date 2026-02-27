using MediatR;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.UserJourneys.Queries.GetMyJourneys;

public class GetMyJourneysQueryHandler : IRequestHandler<GetMyJourneysQuery, ApiResponse<List<UserJourneyResponse>>>
{
    private readonly IUserJourneyRepository _journeyRepository;

    public GetMyJourneysQueryHandler(IUserJourneyRepository journeyRepository)
    {
        _journeyRepository = journeyRepository;
    }

    public async Task<ApiResponse<List<UserJourneyResponse>>> Handle(GetMyJourneysQuery request, CancellationToken cancellationToken)
    {
        var journeys = await _journeyRepository.GetByUserIdAsync(request.UserId, request.Page, request.PageSize);

        var response = journeys.Select(j => new UserJourneyResponse
        {
            Id = j.Id,
            UserId = j.UserId,
            StartLatitude = j.RoutePath.StartPoint.Y,
            StartLongitude = j.RoutePath.StartPoint.X,
            EndLatitude = j.Status == JourneyStatus.Completed ? j.RoutePath.EndPoint.Y : null,
            EndLongitude = j.Status == JourneyStatus.Completed ? j.RoutePath.EndPoint.X : null,
            Status = j.Status.ToString(),
            StartedAt = j.CreatedAt
        }).ToList();

        return ApiResponse<List<UserJourneyResponse>>.Success(response);
    }
}