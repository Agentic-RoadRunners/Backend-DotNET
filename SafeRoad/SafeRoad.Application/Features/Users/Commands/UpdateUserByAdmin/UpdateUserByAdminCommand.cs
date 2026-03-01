using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Commands.UpdateUserByAdmin;

public class UpdateUserByAdminCommand : IRequest<ApiResponse<AdminUserResponse>>
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public List<string>? Roles { get; set; }
}
