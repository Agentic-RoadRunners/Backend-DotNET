
namespace SafeRoad.Core.DTOs.Incident;

public class IncidentResponse
{
    public Guid Id { get; set; }
    public Guid ReporterUserId { get; set; }
    public string? ReporterName { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int? MunicipalityId { get; set; }
    public string? MunicipalityName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Status { get; set; } = null!;
    public int PositiveVerifications { get; set; }
    public int NegativeVerifications { get; set; }
    public int CommentCount { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}