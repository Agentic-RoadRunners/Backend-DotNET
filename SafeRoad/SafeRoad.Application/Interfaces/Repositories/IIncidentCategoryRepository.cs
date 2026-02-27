using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IIncidentCategoryRepository : IGenericRepository<IncidentCategory>
{
    Task<IReadOnlyList<IncidentCategory>> GetAllCategoriesAsync();
}