
using System;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IVerificationRepository : IGenericRepository<Verification>
{
    Task<Verification?> GetByUserAndIncidentAsync(Guid userId, Guid incidentId);
    Task<int> GetPositiveCountAsync(Guid incidentId);
    Task<int> GetNegativeCountAsync(Guid incidentId);
}