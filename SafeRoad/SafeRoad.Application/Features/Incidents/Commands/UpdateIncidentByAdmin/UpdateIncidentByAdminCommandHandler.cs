using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentByAdmin;

public class UpdateIncidentByAdminCommandHandler : IRequestHandler<UpdateIncidentByAdminCommand, ApiResponse<IncidentResponse>>
{
    private readonly IIncidentRepository _incidentRepository;

    public UpdateIncidentByAdminCommandHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<IncidentResponse>> Handle(UpdateIncidentByAdminCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetWithDetailsAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        if (request.Title != null) incident.Title = request.Title;
        if (request.Description != null) incident.Description = request.Description;
        if (request.CategoryId.HasValue) incident.CategoryId = request.CategoryId.Value;
        if (request.Status != null && Enum.TryParse<IncidentStatus>(request.Status, true, out var status))
            incident.Status = status;

        await _incidentRepository.UpdateAsync(incident);

        var response = new IncidentResponse
        {
            Id = incident.Id,
            ReporterUserId = incident.ReporterUserId,
            ReporterName = incident.Reporter?.FullName,
            CategoryId = incident.CategoryId,
            CategoryName = incident.Category?.Name ?? "",
            MunicipalityId = incident.MunicipalityId,
            MunicipalityName = incident.Municipality?.Name,
            Title = incident.Title,
            Description = incident.Description,
            Latitude = incident.Location.Y,
            Longitude = incident.Location.X,
            Status = incident.Status.ToString(),
            PhotoUrls = incident.Photos?.Select(p => p.BlobUrl).ToList() ?? new(),
            PositiveVerifications = incident.Verifications?.Count(v => v.IsPositive) ?? 0,
            NegativeVerifications = incident.Verifications?.Count(v => !v.IsPositive) ?? 0,
            CommentCount = incident.Comments?.Count ?? 0,
            CreatedAt = incident.CreatedAt
        };

        return ApiResponse<IncidentResponse>.Success(response, "Incident updated successfully.");
    }
}
