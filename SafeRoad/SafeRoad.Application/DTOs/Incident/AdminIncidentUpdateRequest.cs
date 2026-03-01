namespace SafeRoad.Core.DTOs.Incident;

public class AdminIncidentUpdateRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? Status { get; set; }
}
