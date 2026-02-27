using MediatR;
using SafeRoad.Core.DTOs.Verification;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Verifications.Queries.GetVerifications;

public class GetVerificationsQuery : IRequest<ApiResponse<VerificationSummaryResponse>>
{
    public Guid IncidentId { get; set; }
}