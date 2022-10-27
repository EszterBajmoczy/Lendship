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

        public TokenValidator(ITokenService tokenService, JwtSecurityTokenHandler tokenHandler)
        {
            _tokenService = tokenService;
            _tokenHandler = tokenHandler;
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get => _maxTokenSizeInBytes; set => _maxTokenSizeInBytes = value; }

        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var isDeactivated = _tokenService.IsCurrentTokenDeactivated();
            if (isDeactivated)
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
