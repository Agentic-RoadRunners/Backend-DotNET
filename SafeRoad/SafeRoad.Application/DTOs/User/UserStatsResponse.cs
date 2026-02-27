
namespace SafeRoad.Core.DTOs.User;

public class UserStatsResponse
{
    public Guid UserId { get; set; }
    public int TotalIncidents { get; set; }
    public int TotalComments { get; set; }
    public int TotalVerifications { get; set; }
    public int TrustScore { get; set; }
    public DateTime MemberSince { get; set; }
}