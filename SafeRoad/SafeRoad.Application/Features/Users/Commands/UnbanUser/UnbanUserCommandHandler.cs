using MediatR;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.UnbanUser;

public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommand, ApiResponse<string>>
{
    private readonly IUserRepository _userRepository;

    public UnbanUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<string>> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        if (user.Status == UserStatus.Active)
            return ApiResponse<string>.Fail("User is already active.");

        user.Status = UserStatus.Active;
        await _userRepository.UpdateAsync(user);

        return ApiResponse<string>.Success("User unbanned successfully.");
    }
}
