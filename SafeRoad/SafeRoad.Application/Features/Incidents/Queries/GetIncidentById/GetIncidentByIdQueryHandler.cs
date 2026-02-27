
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, ApiResponse<IncidentDetailResponse>>
{
    private readonly IIncidentRepository _incidentRepository;

    public GetIncidentByIdQueryHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<IncidentDetailResponse>> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetWithDetailsAsync(request.Id);
        if (incident == null)
            throw new NotFoundException("Incident", request.Id);

        var response = new IncidentDetailResponse
        {
            Id = incident.Id,
            ReporterUserId = incident.ReporterUserId,
            ReporterName = incident.Reporter?.FullName,
            CategoryId = incident.CategoryId,
            CategoryName = incident.Category.Name,
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
            CreatedAt = incident.CreatedAt,
            Comments = incident.Comments?.Select(c => new CommentDto
            {
                Id = c.Id,
                UserId = c.UserId,
                UserName = c.User?.FullName,
                Content = c.Content,
                CreatedAt = c.CreatedAt
            }).ToList() ?? new(),
            Verifications = incident.Verifications?.Select(v => new VerificationDto
            {
                Id = v.Id,
                UserId = v.UserId,
                UserName = v.User?.FullName,
                IsPositive = v.IsPositive,
                CreatedAt = v.CreatedAt
            }).ToList() ?? new()
        };

        return ApiResponse<IncidentDetailResponse>.Success(response);
    }
}