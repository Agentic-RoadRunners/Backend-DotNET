using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<ApiResponse<List<AdminUserResponse>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
}
