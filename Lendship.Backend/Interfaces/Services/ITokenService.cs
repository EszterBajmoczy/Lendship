using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface ITokenService
    {
        bool IsCurrentTokenValid();

        bool IsRefreshTokenValid(string refreshToken);

        bool IsPasswordTokenValid(string pswToken);

        Task DeactivateCurrentTokenAndRefreshTokenAsync(string refreshToken);

        JwtSecurityToken GenerateNewToken(List<Claim> authClaims, bool isRefresh);

        JwtSecurityToken GenerateNewPasswordToken(List<Claim> authClaims);
    }
}
