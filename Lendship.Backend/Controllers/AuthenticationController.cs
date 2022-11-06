using Lendship.Backend.Authentication;
using Lendship.Backend.DTO.Authentication;
using Lendship.Backend.DTO.Authentication.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
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
        private readonly IEmailService _emailService;

        private JwtSecurityTokenHandler _tokenHandler;

        private readonly CustomTokenValidationParameters _validatorParameter;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ITokenService tokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _emailService = emailService;

            _validatorParameter = new CustomTokenValidationParameters(configuration);
            _tokenHandler = new JwtSecurityTokenHandler();
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
                Credit = 1000,
                EmailNotificationsEnabled = true,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Registration = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again. Error: " + result.Errors.FirstOrDefault().Description });
            
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
                    refreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                    image = user.ImageLocation
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            if (_tokenService.IsRefreshTokenDeactivated(model.RefreshToken))
            {
                return Unauthorized("Invalid refresh token");
            }
            
            try
            {
                _tokenHandler.ValidateToken(model.RefreshToken, _validatorParameter.GetTokenValidationParameters(), out SecurityToken validatedToken);

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
                    return Unauthorized("Invalid refresh token");
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
                    image = user.ImageLocation
                });
            }
            catch (Exception e)
            {
                return Unauthorized("Invalid refresh token");
            }
            
        }

        [HttpPost("logout")]
        public async Task<IActionResult> DeaktivateToken([FromBody] RefreshTokenModel model)
        {
            await _tokenService.DeactivateCurrentTokenAndRefreshTokenAsync(model.RefreshToken);
            return NoContent();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found!" });
            }

            var pswToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontendUrl = _configuration.GetSection("Frontend").GetValue("ChangePswUrl", "");

            var url = frontendUrl + "?email=" + user.Email + "&token=" + pswToken;

            _emailService.SendEmailAsync(user.UserName, model.Email, url);

            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not found!" });
            } else if (model.Password != model.ConfirmPassword)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "The two passwords not match!" });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password change failed." });
            
            return Ok();
        }
    }
}
