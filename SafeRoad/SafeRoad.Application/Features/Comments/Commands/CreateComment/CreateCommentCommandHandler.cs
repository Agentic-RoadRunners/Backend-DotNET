using MediatR;
using SafeRoad.Core.DTOs.Comment;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ApiResponse<CommentResponse>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IIncidentRepository _incidentRepository;
    private readonly IUserRepository _userRepository;

    public CreateCommentCommandHandler(ICommentRepository commentRepository, IIncidentRepository incidentRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _incidentRepository = incidentRepository;
        _userRepository = userRepository;
    }

    public async Task<ApiResponse<CommentResponse>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        var user = await _userRepository.GetByIdAsync(request.UserId);

        var comment = new Comment
        {
            IncidentId = request.IncidentId,
            UserId = request.UserId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(comment);

        var response = new CommentResponse
        {
            Id = comment.Id,
            IncidentId = comment.IncidentId,
            UserId = comment.UserId,
            UserName = user?.FullName,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };

        return ApiResponse<CommentResponse>.Success(response, "Comment added successfully.");
    }
}