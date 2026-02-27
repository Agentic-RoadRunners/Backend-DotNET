namespace SafeRoad.Core.DTOs.Verification;

public class VerificationResponse
{
    public int Id { get; set; }
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public bool IsPositive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class VerificationSummaryResponse
{
    public Guid IncidentId { get; set; }
    public int PositiveCount { get; set; }
    public int NegativeCount { get; set; }
    public List<VerificationResponse> Verifications { get; set; } = new();
}