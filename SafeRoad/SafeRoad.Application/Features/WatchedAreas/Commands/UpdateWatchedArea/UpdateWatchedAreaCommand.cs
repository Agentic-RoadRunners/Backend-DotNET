
using MediatR;
using SafeRoad.Core.DTOs.WatchedArea;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Commands.UpdateWatchedArea;

public class UpdateWatchedAreaCommand : IRequest<ApiResponse<WatchedAreaResponse>>
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? Label { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? RadiusInMeters { get; set; }
}