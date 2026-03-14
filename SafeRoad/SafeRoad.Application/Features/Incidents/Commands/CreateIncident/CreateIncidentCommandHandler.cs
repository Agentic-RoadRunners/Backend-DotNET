
using MediatR;
using NetTopologySuite.Geometries;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, ApiResponse<IncidentResponse>>
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly ICrewAnalysisService _crewAnalysisService;

    public CreateIncidentCommandHandler(
        IIncidentRepository incidentRepository,
        ICrewAnalysisService crewAnalysisService)
    {
        _incidentRepository = incidentRepository;
        _crewAnalysisService = crewAnalysisService;
    }

    public async Task<ApiResponse<IncidentResponse>> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = new Incident
        {
            Id = Guid.NewGuid(),
            ReporterUserId = request.UserId,
            CategoryId = request.CategoryId,
            MunicipalityId = request.MunicipalityId,
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            Location = new Point(request.Longitude, request.Latitude) { SRID = 4326 },
            Status = IncidentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _incidentRepository.AddAsync(incident);

        // Fire-and-forget: CrewAI KG enrichment arka planda başlar, kullanıcı beklemez.
        _ = _crewAnalysisService.NotifyIncidentCreatedAsync(
            incidentId:  incident.Id,
            title:       incident.Title,
            description: incident.Description,
            category:    incident.CategoryId.ToString(),
            latitude:    request.Latitude,
            longitude:   request.Longitude,
            status:      incident.Status.ToString()
        );

        var response = new IncidentResponse
        {
            Id = incident.Id,
            ReporterUserId = incident.ReporterUserId,
            CategoryId = incident.CategoryId,
            MunicipalityId = incident.MunicipalityId,
            Title = incident.Title,
            Description = incident.Description,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Status = incident.Status.ToString(),
            CreatedAt = incident.CreatedAt
        };

        return ApiResponse<IncidentResponse>.Success(response, "Incident reported successfully.");
    }
}
