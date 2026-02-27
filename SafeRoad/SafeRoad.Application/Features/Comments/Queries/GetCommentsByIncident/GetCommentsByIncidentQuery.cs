using MediatR;
using SafeRoad.Core.DTOs.Comment;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Comments.Queries.GetCommentsByIncident;

public class GetCommentsByIncidentQuery : IRequest<ApiResponse<List<CommentResponse>>>
{
    public Guid IncidentId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}