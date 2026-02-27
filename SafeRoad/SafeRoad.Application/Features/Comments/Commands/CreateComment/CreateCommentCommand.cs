using MediatR;
using SafeRoad.Core.DTOs.Comment;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Comments.Commands.CreateComment;

public class CreateCommentCommand : IRequest<ApiResponse<CommentResponse>>
{
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = null!;
}