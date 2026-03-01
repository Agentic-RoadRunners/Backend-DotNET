
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;

namespace SafeRoad.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetWithRolesAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateTrustScoreAsync(Guid userId, int scoreChange)
    {
        var user = await _dbSet.FindAsync(userId);
        if (user != null)
        {
            user.TrustScore += scoreChange;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IReadOnlyList<User>> GetAllPaginatedWithRolesAsync(int page, int pageSize, string? search = null, string? role = null, string? status = null)
    {
        var query = _dbSet
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(u => u.Email.ToLower().Contains(s) || (u.FullName != null && u.FullName.ToLower().Contains(s)));
        }

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(u => u.UserRoles.Any(ur => ur.Role.Name == role));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(u => u.Status.ToString() == status);

        return await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetFilteredCountAsync(string? search = null, string? role = null, string? status = null)
    {
        var query = _dbSet
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.ToLower();
            query = query.Where(u => u.Email.ToLower().Contains(s) || (u.FullName != null && u.FullName.ToLower().Contains(s)));
        }

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(u => u.UserRoles.Any(ur => ur.Role.Name == role));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(u => u.Status.ToString() == status);

        return await query.CountAsync();
    }
}