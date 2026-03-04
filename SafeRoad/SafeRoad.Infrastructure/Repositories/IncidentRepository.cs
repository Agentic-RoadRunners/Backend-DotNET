
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Enums;
namespace SafeRoad.Infrastructure.Repositories;

public class IncidentRepository : GenericRepository<Incident>, IIncidentRepository
{
    public IncidentRepository(SafeRoadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Incident>> GetAllPaginatedAsync(int page, int pageSize)
    {
        return await _dbSet
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.Category)
            .Include(i => i.Reporter)
            .Include(i => i.Municipality)
            .Include(i => i.Photos)
            .Include(i => i.Verifications)
            .Include(i => i.Comments)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Incident>> GetNearbyAsync(double latitude, double longitude, int radiusMeters, int page, int pageSize)
    {
        var userLocation = new Point(longitude, latitude) { SRID = 4326 };

        return await _dbSet
            .Where(i => i.Location.Distance(userLocation) <= radiusMeters)
            .OrderBy(i => i.Location.Distance(userLocation))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.Category)
            .Include(i => i.Reporter)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Incident>> GetByUserIdAsync(Guid userId, int page, int pageSize)
    {
        return await _dbSet
            .Where(i => i.ReporterUserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.Category)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Incident>> GetByCategoryAsync(int categoryId, int page, int pageSize)
    {
        return await _dbSet
            .Where(i => i.CategoryId == categoryId)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.Category)
            .Include(i => i.Reporter)
            .ToListAsync();
    }

    public async Task<Incident?> GetWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.Reporter)
            .Include(i => i.Municipality)
            .Include(i => i.Photos)
            .Include(i => i.Comments)
            .Include(i => i.Verifications)
            .AsSplitQuery()
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Incident>> GetAlongRouteAsync(LineString route, int bufferMeters = 500)
    {
        return await _context.Incidents
            .Include(i => i.Category)
            .Include(i => i.Reporter)
            .Where(i => i.Location.Distance(route) <= bufferMeters)
            .Where(i => i.Status != IncidentStatus.Resolved)
            .OrderBy(i => i.Location.Distance(route))
            .Take(50)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Incident>> GetByMunicipalityPaginatedAsync(int municipalityId, int page, int pageSize)
    {
        return await _dbSet
            .Where(i => i.MunicipalityId == municipalityId)
            .OrderByDescending(i => i.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.Category)
            .Include(i => i.Reporter)
            .Include(i => i.Municipality)
            .Include(i => i.Photos)
            .Include(i => i.Verifications)
            .Include(i => i.Comments)
            .AsSplitQuery()
            .ToListAsync();
    }
}