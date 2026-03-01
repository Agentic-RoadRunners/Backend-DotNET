using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, ApiResponse<List<AdminUserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly ICommentRepository _commentRepository;

    public GetAllUsersQueryHandler(
        IUserRepository userRepository,
        IIncidentRepository incidentRepository,
        ICommentRepository commentRepository)
    {
        _userRepository = userRepository;
        _incidentRepository = incidentRepository;
        _commentRepository = commentRepository;
    }

    public async Task<ApiResponse<List<AdminUserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllPaginatedWithRolesAsync(
            request.Page, request.PageSize, request.Search, request.Role, request.Status);

        var response = new List<AdminUserResponse>();
        foreach (var user in users)
        {
            response.Add(new AdminUserResponse
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
            });
        }

        return ApiResponse<List<AdminUserResponse>>.Success(response);
    }
}
