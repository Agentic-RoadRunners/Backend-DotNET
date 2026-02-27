namespace SafeRoad.Core.DTOs.Analytics;

public class AnalyticsOverviewResponse
{
    public int TotalIncidents { get; set; }
    public int TotalUsers { get; set; }
    public int ResolvedCount { get; set; }
    public int MunicipalityCount { get; set; }
}
