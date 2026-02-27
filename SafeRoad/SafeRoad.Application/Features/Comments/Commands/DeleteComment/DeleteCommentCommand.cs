using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommand : IRequest<ApiResponse<string>>
{
    public int CommentId { get; set; }
    public Guid UserId { get; set; }
}