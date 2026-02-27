
using MediatR;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Users.Queries.GetUserStats;

public class GetUserStatsQueryHandler : IRequestHandler<GetUserStatsQuery, ApiResponse<UserStatsResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IVerificationRepository _verificationRepository;

    public GetUserStatsQueryHandler(IUserRepository userRepository, IIncidentRepository incidentRepository, ICommentRepository commentRepository, IVerificationRepository verificationRepository)
    {
        _userRepository = userRepository;
        _incidentRepository = incidentRepository;
        _commentRepository = commentRepository;
        _verificationRepository = verificationRepository;
    }

    public async Task<ApiResponse<UserStatsResponse>> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException("User", request.UserId);

        var response = new UserStatsResponse
        {
            UserId = user.Id,
            TotalIncidents = await _incidentRepository.CountAsync(i => i.ReporterUserId == request.UserId),
            TotalComments = await _commentRepository.CountAsync(c => c.UserId == request.UserId),
            TotalVerifications = await _verificationRepository.CountAsync(v => v.UserId == request.UserId),
            TrustScore = user.TrustScore,
            MemberSince = user.CreatedAt
        };

        return ApiResponse<UserStatsResponse>.Success(response);
    }
}