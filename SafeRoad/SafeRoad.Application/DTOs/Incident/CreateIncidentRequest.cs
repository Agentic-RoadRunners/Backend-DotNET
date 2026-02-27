
namespace SafeRoad.Core.DTOs.Incident;

public class CreateIncidentRequest
{
    public int CategoryId { get; set; }
    public int? MunicipalityId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}