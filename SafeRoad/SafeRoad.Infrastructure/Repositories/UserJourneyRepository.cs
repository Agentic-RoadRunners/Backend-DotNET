
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Interfaces.Repositories;

namespace SafeRoad.Infrastructure.Repositories;

public class UserJourneyRepository : GenericRepository<UserJourney>, IUserJourneyRepository
{
    public UserJourneyRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<UserJourney?> GetActiveByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(j => j.UserId == userId && j.Status == JourneyStatus.Active);
    }

    public async Task<IReadOnlyList<UserJourney>> GetByUserIdAsync(Guid userId, int page, int pageSize)
    {
        return await _dbSet
            .Where(j => j.UserId == userId)
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}