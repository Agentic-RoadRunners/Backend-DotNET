using MediatR;
using SafeRoad.Core.DTOs.IncidentCategory;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.IncidentCategories.Queries.GetAllIncidentCategories;

public class GetAllIncidentCategoriesQuery : IRequest<ApiResponse<List<IncidentCategoryResponse>>>
{
}