
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IWatchedAreaRepository : IGenericRepository<WatchedArea>
{
    Task<IReadOnlyList<WatchedArea>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<WatchedArea>> GetAreasContainingPointAsync(double latitude, double longitude);
}