
using System.Collections.Generic;
using SafeRoad.Core.Entities;

namespace SafeRoad.Core.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user, IList<string> roles);
}