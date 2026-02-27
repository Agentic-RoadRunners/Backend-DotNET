using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Verifications.Commands.RemoveVerification;

public class RemoveVerificationCommand : IRequest<ApiResponse<string>>
{
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
}