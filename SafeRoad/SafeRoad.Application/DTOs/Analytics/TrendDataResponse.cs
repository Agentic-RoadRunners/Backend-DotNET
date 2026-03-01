namespace SafeRoad.Core.DTOs.Analytics;

public class TrendDataResponse
{
    public string Date { get; set; } = null!;
    public int Count { get; set; }
    public int Resolved { get; set; }
}
