using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lendship.Backend.Authentication
{
    public class CustomTokenValidationParameters
    {
        private TokenValidationParameters _tokenValidationParameters;

        public CustomTokenValidationParameters(IConfiguration configuration)
        {
            _tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("JWT").GetValue("Audience", "defaultAudience"),
                ValidIssuer = configuration.GetSection("JWT").GetValue("Issuer", "defaultIssuer"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT").GetValue("Key", "defaultKey")))
            };
        }

        public TokenValidationParameters GetTokenValidationParameters() => _tokenValidationParameters;
    }
}
