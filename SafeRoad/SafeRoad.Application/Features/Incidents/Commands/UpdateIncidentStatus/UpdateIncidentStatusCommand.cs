
using System.Text.Json.Serialization;
using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommand : IRequest<ApiResponse<string>>
{
    /// <summary>
    /// Set automatically from the route parameter — do not send in the request body.
    /// </summary>
    [JsonIgnore]
    public Guid IncidentId { get; set; }

    /// <summary>
    /// Status name as string (e.g. "Pending", "Verified", "Disputed", "Resolved").
    /// </summary>
    public string NewStatus { get; set; } = null!;
}