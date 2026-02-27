using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;

namespace SafeRoad.Infrastructure.Repositories;

public class IncidentCategoryRepository : GenericRepository<IncidentCategory>, IIncidentCategoryRepository
{
    public IncidentCategoryRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<IncidentCategory>> GetAllCategoriesAsync()
    {
        return await _dbSet
            .OrderBy(c => c.Id)
            .ToListAsync();
    }
}