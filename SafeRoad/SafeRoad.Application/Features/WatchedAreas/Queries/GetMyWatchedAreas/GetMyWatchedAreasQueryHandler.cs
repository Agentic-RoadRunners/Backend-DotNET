
using MediatR;
using SafeRoad.Core.DTOs.WatchedArea;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Queries.GetMyWatchedAreas;

public class GetMyWatchedAreasQueryHandler : IRequestHandler<GetMyWatchedAreasQuery, ApiResponse<List<WatchedAreaResponse>>>
{
    private readonly IWatchedAreaRepository _watchedAreaRepository;

    public GetMyWatchedAreasQueryHandler(IWatchedAreaRepository watchedAreaRepository)
    {
        _watchedAreaRepository = watchedAreaRepository;
    }

    public async Task<ApiResponse<List<WatchedAreaResponse>>> Handle(GetMyWatchedAreasQuery request, CancellationToken cancellationToken)
    {
        var areas = await _watchedAreaRepository.GetByUserIdAsync(request.UserId);

        var response = areas.Select(a => new WatchedAreaResponse
        {
            Id = a.Id,
            Label = a.Label,
            Latitude = a.Area.Y,
            Longitude = a.Area.X,
            RadiusInMeters = a.RadiusInMeters
        }).ToList();

        return ApiResponse<List<WatchedAreaResponse>>.Success(response);
    }
}