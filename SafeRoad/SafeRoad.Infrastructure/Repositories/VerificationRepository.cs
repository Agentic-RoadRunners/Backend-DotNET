
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;

namespace SafeRoad.Infrastructure.Repositories;

public class VerificationRepository : GenericRepository<Verification>, IVerificationRepository
{
    public VerificationRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<Verification?> GetByUserAndIncidentAsync(Guid userId, Guid incidentId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(v => v.UserId == userId && v.IncidentId == incidentId);
    }

    public async Task<int> GetPositiveCountAsync(Guid incidentId)
    {
        return await _dbSet
            .CountAsync(v => v.IncidentId == incidentId && v.IsPositive);
    }

    public async Task<int> GetNegativeCountAsync(Guid incidentId)
    {
        return await _dbSet
            .CountAsync(v => v.IncidentId == incidentId && !v.IsPositive);
    }
}