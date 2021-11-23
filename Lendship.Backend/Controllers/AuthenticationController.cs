using Lendship.Backend.Authentication;
using Lendship.Backend.DTO.Authentication;
using Lendship.Backend.DTO.Authentication.Authentication;
using Lendship.Backend.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

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

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
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

                var token = _tokenService.GenerateNewToken(authClaims, false);
                var refreshToken = _tokenService.GenerateNewToken(authClaims, true);

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
            if (!_tokenService.IsRefreshTokenValid(model.RefreshToken))
            {
                return Unauthorized();
            }
            var refreshToken = new JwtSecurityToken(model.RefreshToken);
            var userId = refreshToken.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userId.Value);

            var audience = refreshToken.Audiences.FirstOrDefault();
            var validAudience = _configuration.GetSection("JWT").GetValue("Audience", "defaultAudience");

            var issuer = refreshToken.Issuer;
            var validIssuer = _configuration.GetSection("JWT").GetValue("Issuer", "defaultIssuer");

            if (refreshToken.ValidTo < DateTime.Now 
                || user == null
                || audience != validAudience
                || issuer != validIssuer)
            {
                return Unauthorized();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
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

            var token = _tokenService.GenerateNewToken(authClaims, false);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> DeaktivateToken([FromBody] RefreshTokenModel model)
        {
            await _tokenService.DeactivateCurrentTokenAndRefreshTokenAsync(model.RefreshToken);
            return NoContent();
        }
    }
}
