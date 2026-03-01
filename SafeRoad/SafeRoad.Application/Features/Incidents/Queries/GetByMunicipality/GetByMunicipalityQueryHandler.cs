using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetByMunicipality;

public class GetByMunicipalityQueryHandler : IRequestHandler<GetByMunicipalityQuery, ApiResponse<List<IncidentResponse>>>
{
    private readonly IIncidentRepository _incidentRepository;

    public GetByMunicipalityQueryHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<List<IncidentResponse>>> Handle(GetByMunicipalityQuery request, CancellationToken cancellationToken)
    {
        var incidents = await _incidentRepository.GetByMunicipalityPaginatedAsync(
            request.MunicipalityId, request.Page, request.PageSize);

        var response = incidents.Select(i => new IncidentResponse
        {
            Id = i.Id,
            ReporterUserId = i.ReporterUserId,
            ReporterName = i.Reporter?.FullName,
            CategoryId = i.CategoryId,
            CategoryName = i.Category?.Name ?? "",
            MunicipalityId = i.MunicipalityId,
            MunicipalityName = i.Municipality?.Name,
            Title = i.Title,
            Description = i.Description,
            Latitude = i.Location.Y,
            Longitude = i.Location.X,
            Status = i.Status.ToString(),
            PhotoUrls = i.Photos?.Select(p => p.BlobUrl).ToList() ?? new(),
            PositiveVerifications = i.Verifications?.Count(v => v.IsPositive) ?? 0,
            NegativeVerifications = i.Verifications?.Count(v => !v.IsPositive) ?? 0,
            CommentCount = i.Comments?.Count ?? 0,
            CreatedAt = i.CreatedAt
        }).ToList();

        return ApiResponse<List<IncidentResponse>>.Success(response);
    }
}
