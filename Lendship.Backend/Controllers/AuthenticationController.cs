using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.DTO.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lendship.Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Name,
                Credit = 0,
                EmailNotificationsEnabled = true,
                Latitude = 3,
                Longitude = 2,
                Registration = DateTime.Now
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = generateToken(authClaims, false);
                authClaims.Add(new Claim(ClaimTypes.AuthenticationMethod, "RefreshToken"));

                //not sure, but that way the refresh token won't work as a normal token (issuer)
                var refreshToken = generateToken(authClaims, true);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    refreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken)
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            var refreshToken = new JwtSecurityToken(model.RefreshToken);
            var userId = refreshToken.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            var user = await userManager.FindByIdAsync(userId.Value);

            var audience = refreshToken.Audiences.FirstOrDefault();
            var validAudience = _configuration.GetSection("JWT").GetValue("Audience", "defaultAudience");

            var issuer = refreshToken.Issuer;
            var validIssuer = _configuration.GetSection("JWT").GetValue("Issuer", "defaultIssuer");

            var issuerSigningKey = refreshToken.SigningKey;
            var validIssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT").GetValue("Key", "defaultKey")));

            if (refreshToken.ValidTo < DateTime.Now 
                || user == null
                || audience != validAudience
                || issuer != validIssuer
                || issuerSigningKey != validIssuerSigningKey)
            {
                return Unauthorized();
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = generateToken(authClaims, false);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }

        private JwtSecurityToken generateToken(List<Claim> authClaims, bool isRefresh)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT").GetValue("Key", "defaultKey")));
            var issuer = isRefresh ?
                _configuration.GetSection("JWT").GetValue("Issuer", "defaultIssuer") + "refresh" :
                _configuration.GetSection("JWT").GetValue("Issuer", "defaultIssuer");
            var expires = isRefresh ? 6 : 3;

            return new JwtSecurityToken(
                issuer: issuer,
                audience: _configuration.GetSection("JWT").GetValue("Audience", "defaultAudience"),
                expires: DateTime.Now.AddHours(expires),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
