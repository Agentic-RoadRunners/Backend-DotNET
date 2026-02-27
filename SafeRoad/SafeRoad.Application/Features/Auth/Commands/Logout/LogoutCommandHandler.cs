using MediatR;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<string>>
{
    private readonly IGenericRepository<SafeRoad.Core.Entities.DeviceToken> _deviceTokenRepository;

    public LogoutCommandHandler(IGenericRepository<SafeRoad.Core.Entities.DeviceToken> deviceTokenRepository)
    {
        _deviceTokenRepository = deviceTokenRepository;
    }

    public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _deviceTokenRepository.FindAsync(t => t.UserId == request.UserId);
        foreach (var token in tokens)
        {
            await _deviceTokenRepository.DeleteAsync(token);
        }

        return ApiResponse<string>.Success("Logged out successfully. Please discard your token on client side.");
    }
}