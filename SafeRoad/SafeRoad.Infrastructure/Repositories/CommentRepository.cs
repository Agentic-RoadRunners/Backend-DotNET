
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;

namespace SafeRoad.Infrastructure.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Comment>> GetByIncidentIdAsync(Guid incidentId, int page, int pageSize)
    {
        return await _dbSet
            .Where(c => c.IncidentId == incidentId)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.User)
            .ToListAsync();
    }
}