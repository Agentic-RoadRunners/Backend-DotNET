
using MediatR;
using SafeRoad.Core.DTOs.WatchedArea;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Queries.GetMyWatchedAreas;

public class GetMyWatchedAreasQuery : IRequest<ApiResponse<List<WatchedAreaResponse>>>
{
    public Guid UserId { get; set; }
}