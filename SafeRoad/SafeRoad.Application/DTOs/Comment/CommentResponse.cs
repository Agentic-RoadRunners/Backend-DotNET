namespace SafeRoad.Core.DTOs.Comment;

public class CommentResponse
{
    public int Id { get; set; }
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}