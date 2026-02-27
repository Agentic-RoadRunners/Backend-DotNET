
using MediatR;
using NetTopologySuite.Geometries;
using SafeRoad.Core.DTOs.WatchedArea;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Commands.UpdateWatchedArea;

public class UpdateWatchedAreaCommandHandler : IRequestHandler<UpdateWatchedAreaCommand, ApiResponse<WatchedAreaResponse>>
{
    private readonly IWatchedAreaRepository _watchedAreaRepository;

    public UpdateWatchedAreaCommandHandler(IWatchedAreaRepository watchedAreaRepository)
    {
        _watchedAreaRepository = watchedAreaRepository;
    }

    public async Task<ApiResponse<WatchedAreaResponse>> Handle(UpdateWatchedAreaCommand request, CancellationToken cancellationToken)
    {
        var watchedArea = await _watchedAreaRepository.GetByIdAsync(request.Id);
        if (watchedArea == null)
            throw new NotFoundException("WatchedArea", request.Id);

        if (watchedArea.UserId != request.UserId)
            throw new ForbiddenException("You can only update your own watched areas.");

        if (request.Label != null) watchedArea.Label = request.Label;
        if (request.RadiusInMeters.HasValue) watchedArea.RadiusInMeters = request.RadiusInMeters.Value;
        if (request.Latitude.HasValue && request.Longitude.HasValue)
            watchedArea.Area = new Point(request.Longitude.Value, request.Latitude.Value) { SRID = 4326 };

        await _watchedAreaRepository.UpdateAsync(watchedArea);

        var response = new WatchedAreaResponse
        {
            Id = watchedArea.Id,
            Label = watchedArea.Label,
            Latitude = watchedArea.Area.Y,
            Longitude = watchedArea.Area.X,
            RadiusInMeters = watchedArea.RadiusInMeters
        };

        return ApiResponse<WatchedAreaResponse>.Success(response, "Watched area updated successfully.");
    }
}