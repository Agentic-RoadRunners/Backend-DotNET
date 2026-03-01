using MediatR;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.BanUser;

public class BanUserCommandHandler : IRequestHandler<BanUserCommand, ApiResponse<string>>
{
    private readonly IUserRepository _userRepository;

    public BanUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<string>> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        if (user.Status == UserStatus.Banned)
            return ApiResponse<string>.Fail("User is already banned.");

        user.Status = UserStatus.Banned;
        await _userRepository.UpdateAsync(user);

        return ApiResponse<string>.Success("User banned successfully.");
    }
}
