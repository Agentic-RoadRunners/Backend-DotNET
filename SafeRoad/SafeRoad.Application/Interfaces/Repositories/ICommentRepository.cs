
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IReadOnlyList<Comment>> GetByIncidentIdAsync(Guid incidentId, int page, int pageSize);
}