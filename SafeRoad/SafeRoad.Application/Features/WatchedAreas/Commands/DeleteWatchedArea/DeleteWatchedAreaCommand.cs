
using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Commands.DeleteWatchedArea;

public class DeleteWatchedAreaCommand : IRequest<ApiResponse<string>>
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
}