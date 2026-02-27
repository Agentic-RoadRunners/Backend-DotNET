
using System.Text.Json.Serialization;
using MediatR;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommand : IRequest<ApiResponse<string>>
{
    /// <summary>
    /// Set automatically from the route parameter — do not send in the request body.
    /// </summary>
    [JsonIgnore]
    public Guid IncidentId { get; set; }
    public IncidentStatus NewStatus { get; set; }
}