
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IIncidentRepository : IGenericRepository<Incident>
{
    Task<IReadOnlyList<Incident>> GetAllPaginatedAsync(int page, int pageSize);
    Task<IReadOnlyList<Incident>> GetNearbyAsync(double latitude, double longitude, int radiusMeters, int page, int pageSize);
    Task<IReadOnlyList<Incident>> GetByUserIdAsync(Guid userId, int page, int pageSize);
    Task<IReadOnlyList<Incident>> GetByCategoryAsync(int categoryId, int page, int pageSize);
    Task<Incident?> GetWithDetailsAsync(Guid id);

    Task<IEnumerable<Incident>> GetAlongRouteAsync(NetTopologySuite.Geometries.LineString route, int bufferMeters = 500);
}