using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lendship.Backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDistributedCache _redisCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;


        public TokenService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _redisCache = cache;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            _logger = new Logger.Logger(_configuration);
        }

        public bool IsCurrentTokenValid()
        {
            var currentToken = GetCurrentToken();
            try
            {
                return _redisCache.GetString(currentToken) == null;
            } catch (Exception e)
            {
                _logger.Error("Error validating token: " + e.Message);
                return true;
            };
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            try
            {
                return _redisCache.GetString($"ref_{refreshToken}") == null;
            }
            catch (Exception e)
            {
                _logger.Error("Error validating refresh token: " + e.Message);
                return true;
            };
        }

        public async Task DeactivateCurrentTokenAndRefreshTokenAsync(string refreshToken)
        {
            var currentToken = GetCurrentToken();
            var expires = new JwtSecurityToken(currentToken).ValidTo;
            var expiresRefreshToken = new JwtSecurityToken(refreshToken).ValidTo;
            try
            {
                await _redisCache.SetStringAsync(
                        currentToken,
                        "deactivated",
                        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromTicks(expires.Ticks - DateTime.Now.Ticks) },
                        CancellationToken.None);

                await _redisCache.SetStringAsync(
                            refreshToken,
                            "deactivated",
                            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromTicks(expiresRefreshToken.Ticks - DateTime.Now.Ticks) },
                            CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.Error("Error deactivating tokens: " + e.Message);
            };
            
        }

        public JwtSecurityToken GenerateNewToken(List<Claim> authClaims, bool isRefresh)
        {
            var jwtToken = generate(authClaims, isRefresh);
            var token = isRefresh ?
                $"ref_{new JwtSecurityTokenHandler().WriteToken(jwtToken)}" :
                new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var expires = jwtToken.ValidTo;

            return jwtToken;
        }

        private JwtSecurityToken generate(List<Claim> authClaims, bool isRefresh)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT").GetValue("Key", "defaultKey")));
            var issuer = _configuration.GetSection("JWT").GetValue("Issuer", "defaultIssuer");
            var expires = isRefresh ?
                _configuration.GetSection("JWT").GetValue("ExpirationRefresh", 1) :
                _configuration.GetSection("JWT").GetValue("Expiration", 0.5);

            return new JwtSecurityToken(
                issuer: issuer,
                audience: _configuration.GetSection("JWT").GetValue("Audience", "defaultAudience"),
                expires: DateTime.Now.AddHours(expires),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        private string GetCurrentToken()
        {
            var token = _httpContextAccessor
                            .HttpContext
                            .Request
                            .Headers["authorization"]
                            .ToString()
                            .Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new MissingTokenException("Error at getting current token");
            }

            return token;
        }
    }
}
