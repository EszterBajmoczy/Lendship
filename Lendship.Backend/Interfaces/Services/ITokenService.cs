using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface ITokenService
    {
        bool IsCurrentTokenValid();

        bool IsRefreshTokenValid(string refreshToken);

        Task DeactivateCurrentTokenAndRefreshTokenAsync(string refreshToken);

        JwtSecurityToken GenerateNewToken(List<Claim> authClaims, bool isRefresh);
    }
}
