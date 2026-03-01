
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetWithRolesAsync(Guid id);
    Task UpdateTrustScoreAsync(Guid userId, int scoreChange);
    Task<IReadOnlyList<User>> GetAllPaginatedWithRolesAsync(int page, int pageSize, string? search = null, string? role = null, string? status = null);
    Task<int> GetFilteredCountAsync(string? search = null, string? role = null, string? status = null);
}