using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Lendship.Backend.Authentication
{
    public class TokenValidator : ISecurityTokenValidator
    {
        private int _maxTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        private ITokenService _tokenService;
        private JwtSecurityTokenHandler _tokenHandler;

        public TokenValidator(ITokenService tokenService)
        {
            _tokenService = tokenService;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get => _maxTokenSizeInBytes; set => _maxTokenSizeInBytes = value; }

        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var valid = _tokenService.IsCurrentTokenValid();
            if (!valid)
            {
                throw new InvalidTokenException("Error by validating the token.");
            }
            
            try
            {
                var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
                return principal;
            } catch (Exception e)
            {
                throw new InvalidTokenException("Error by validating the token: " + e);
            }            
        }
    }
}
