namespace SafeRoad.Core.DTOs.Analytics;

public class CategoryStatsResponse
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
