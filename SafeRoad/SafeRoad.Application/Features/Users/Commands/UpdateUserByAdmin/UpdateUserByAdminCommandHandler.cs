using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.UpdateUserByAdmin;

public class UpdateUserByAdminCommandHandler : IRequestHandler<UpdateUserByAdminCommand, ApiResponse<AdminUserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly ICommentRepository _commentRepository;

    public UpdateUserByAdminCommandHandler(
        IUserRepository userRepository,
        IIncidentRepository incidentRepository,
        ICommentRepository commentRepository)
    {
        _userRepository = userRepository;
        _incidentRepository = incidentRepository;
        _commentRepository = commentRepository;
    }

    public async Task<ApiResponse<AdminUserResponse>> Handle(UpdateUserByAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetWithRolesAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        if (request.FullName != null)
            user.FullName = request.FullName;

        await _userRepository.UpdateAsync(user);

        var response = new AdminUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            TrustScore = user.TrustScore,
            Status = user.Status.ToString(),
            Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new(),
            TotalIncidents = await _incidentRepository.CountAsync(i => i.ReporterUserId == user.Id),
            TotalComments = await _commentRepository.CountAsync(c => c.UserId == user.Id),
            CreatedAt = user.CreatedAt
        };

        return ApiResponse<AdminUserResponse>.Success(response, "User updated successfully.");
    }
}
