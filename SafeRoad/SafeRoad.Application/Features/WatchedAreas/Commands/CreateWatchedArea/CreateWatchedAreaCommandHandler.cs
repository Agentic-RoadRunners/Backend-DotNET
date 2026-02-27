
using MediatR;
using NetTopologySuite.Geometries;
using SafeRoad.Core.DTOs.WatchedArea;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Commands.CreateWatchedArea;

public class CreateWatchedAreaCommandHandler : IRequestHandler<CreateWatchedAreaCommand, ApiResponse<WatchedAreaResponse>>
{
    private readonly IWatchedAreaRepository _watchedAreaRepository;

    public CreateWatchedAreaCommandHandler(IWatchedAreaRepository watchedAreaRepository)
    {
        _watchedAreaRepository = watchedAreaRepository;
    }

    public async Task<ApiResponse<WatchedAreaResponse>> Handle(CreateWatchedAreaCommand request, CancellationToken cancellationToken)
    {
        var watchedArea = new WatchedArea
        {
            UserId = request.UserId,
            Label = request.Label,
            Area = new Point(request.Longitude, request.Latitude) { SRID = 4326 },
            RadiusInMeters = request.RadiusInMeters
        };

        await _watchedAreaRepository.AddAsync(watchedArea);

        var response = new WatchedAreaResponse
        {
            Id = watchedArea.Id,
            Label = watchedArea.Label,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            RadiusInMeters = watchedArea.RadiusInMeters
        };

        return ApiResponse<WatchedAreaResponse>.Success(response, "Watched area created successfully.");
    }
}