
using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.WatchedAreas.Commands.DeleteWatchedArea;

public class DeleteWatchedAreaCommandHandler : IRequestHandler<DeleteWatchedAreaCommand, ApiResponse<string>>
{
    private readonly IWatchedAreaRepository _watchedAreaRepository;

    public DeleteWatchedAreaCommandHandler(IWatchedAreaRepository watchedAreaRepository)
    {
        _watchedAreaRepository = watchedAreaRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteWatchedAreaCommand request, CancellationToken cancellationToken)
    {
        var watchedArea = await _watchedAreaRepository.GetByIdAsync(request.Id);
        if (watchedArea == null)
            throw new NotFoundException("WatchedArea", request.Id);

        if (watchedArea.UserId != request.UserId)
            throw new ForbiddenException("You can only delete your own watched areas.");

        await _watchedAreaRepository.DeleteAsync(watchedArea);

        return ApiResponse<string>.Success("Watched area deleted successfully.");
    }
}