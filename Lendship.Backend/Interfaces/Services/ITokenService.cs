using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface ITokenService
    {
        bool IsCurrentTokenDeactivated();

        bool IsRefreshTokenDeactivated(string refreshToken);

        Task DeactivateCurrentTokenAndRefreshTokenAsync(string refreshToken);

        JwtSecurityToken GenerateNewToken(List<Claim> authClaims, bool isRefresh);
    }
}
