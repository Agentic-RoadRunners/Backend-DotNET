using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SafeRoad.Core.Interfaces.Services;

namespace SafeRoad.Infrastructure.Services;

/// <summary>
/// FastAPI /crew/analyze endpoint'ini fire-and-forget olarak çağırır.
/// Response beklenmez — ASP.NET kullanıcı isteğini bloke etmez.
/// </summary>
public class CrewAnalysisService : ICrewAnalysisService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CrewAnalysisService> _logger;
    private const string FastApiAnalyzeUrl = "http://localhost:8000/crew/analyze";

    public CrewAnalysisService(HttpClient httpClient, ILogger<CrewAnalysisService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public Task NotifyIncidentCreatedAsync(
        Guid incidentId,
        string title,
        string description,
        string category,
        double latitude,
        double longitude,
        string status = "Pending")
    {
        var payload = new
        {
            incident_id  = incidentId.ToString(),
            title,
            description,
            category,
            lat          = latitude,
            lng          = longitude,
            status
        };

        // Fire-and-forget: Task kasıtlı olarak beklenmez.
        // FastAPI anında 202 döndürür; crew arka planda çalışır.
        _ = _httpClient.PostAsJsonAsync(FastApiAnalyzeUrl, payload);

        _logger.LogInformation(
            "CrewAI KG enrichment tetiklendi: IncidentId={IncidentId}", incidentId);

        return Task.CompletedTask;
    }
}
