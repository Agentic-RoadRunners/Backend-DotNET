namespace SafeRoad.Core.Interfaces.Services;

/// <summary>
/// FastAPI /crew/analyze endpoint'ini fire-and-forget olarak çağırır.
/// Yeni bir incident oluşturulduğunda CrewAI KG enrichment sürecini başlatır.
/// </summary>
public interface ICrewAnalysisService
{
    /// <summary>
    /// FastAPI'ye incident verilerini gönderir. Response beklenmez.
    /// </summary>
    Task NotifyIncidentCreatedAsync(
        Guid incidentId,
        string title,
        string description,
        string category,
        double latitude,
        double longitude,
        string status = "Pending");
}
