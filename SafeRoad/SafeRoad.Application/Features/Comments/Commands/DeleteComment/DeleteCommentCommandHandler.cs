using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, ApiResponse<string>>
{
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);
        if (comment == null)
            throw new NotFoundException("Comment", request.CommentId);

        if (comment.UserId != request.UserId)
            throw new ForbiddenException("You can only delete your own comments.");

        await _commentRepository.DeleteAsync(comment);

        return ApiResponse<string>.Success("Comment deleted successfully.");
    }
}