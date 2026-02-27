using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Verifications.Commands.VerifyIncident;

public class VerifyIncidentCommand : IRequest<ApiResponse<string>>
{
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
}