using MediatR;
using SafeRoad.Core.DTOs.Comment;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Comments.Queries.GetCommentsByIncident;

public class GetCommentsByIncidentQueryHandler : IRequestHandler<GetCommentsByIncidentQuery, ApiResponse<List<CommentResponse>>>
{
    private readonly ICommentRepository _commentRepository;

    public GetCommentsByIncidentQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<ApiResponse<List<CommentResponse>>> Handle(GetCommentsByIncidentQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetByIncidentIdAsync(request.IncidentId, request.Page, request.PageSize);

        var response = comments.Select(c => new CommentResponse
        {
            Id = c.Id,
            IncidentId = c.IncidentId,
            UserId = c.UserId,
            UserName = c.User?.FullName,
            Content = c.Content,
            CreatedAt = c.CreatedAt
        }).ToList();

        return ApiResponse<List<CommentResponse>>.Success(response);
    }
}