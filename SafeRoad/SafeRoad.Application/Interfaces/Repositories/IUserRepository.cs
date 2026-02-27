
using System;
using System.Threading.Tasks;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetWithRolesAsync(Guid id);
    Task UpdateTrustScoreAsync(Guid userId, int scoreChange);
}