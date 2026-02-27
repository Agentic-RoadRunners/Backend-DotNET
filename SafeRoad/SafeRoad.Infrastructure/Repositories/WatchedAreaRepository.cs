
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;

namespace SafeRoad.Infrastructure.Repositories;

public class WatchedAreaRepository : GenericRepository<WatchedArea>, IWatchedAreaRepository
{
    public WatchedAreaRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<WatchedArea>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<WatchedArea>> GetAreasContainingPointAsync(double latitude, double longitude)
    {
        var point = new Point(longitude, latitude) { SRID = 4326 };

        return await _dbSet
            .Where(w => w.Area.Distance(point) <= (w.RadiusInMeters ?? 0))
            .Include(w => w.User)
            .ToListAsync();
    }
}