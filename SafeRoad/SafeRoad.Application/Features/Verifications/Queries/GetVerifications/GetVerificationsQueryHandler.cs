using MediatR;
using SafeRoad.Core.DTOs.Verification;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Verifications.Queries.GetVerifications;

public class GetVerificationsQueryHandler : IRequestHandler<GetVerificationsQuery, ApiResponse<VerificationSummaryResponse>>
{
    private readonly IVerificationRepository _verificationRepository;

    public GetVerificationsQueryHandler(IVerificationRepository verificationRepository)
    {
        _verificationRepository = verificationRepository;
    }

    public async Task<ApiResponse<VerificationSummaryResponse>> Handle(GetVerificationsQuery request, CancellationToken cancellationToken)
    {
        var verifications = await _verificationRepository.FindAsync(v => v.IncidentId == request.IncidentId);

        var response = new VerificationSummaryResponse
        {
            IncidentId = request.IncidentId,
            PositiveCount = verifications.Count(v => v.IsPositive),
            NegativeCount = verifications.Count(v => !v.IsPositive),
            Verifications = verifications.Select(v => new VerificationResponse
            {
                Id = v.Id,
                IncidentId = v.IncidentId,
                UserId = v.UserId,
                IsPositive = v.IsPositive,
                CreatedAt = v.CreatedAt
            }).ToList()
        };

        return ApiResponse<VerificationSummaryResponse>.Success(response);
    }
}