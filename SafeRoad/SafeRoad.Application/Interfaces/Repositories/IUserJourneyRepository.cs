
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IUserJourneyRepository : IGenericRepository<UserJourney>
{
    Task<UserJourney?> GetActiveByUserIdAsync(Guid userId);
    Task<IReadOnlyList<UserJourney>> GetByUserIdAsync(Guid userId, int page, int pageSize);
}