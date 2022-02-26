using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly IUserConverter _userConverter;

        public ProfileService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            _userConverter = new UserConverter();
        }

        public void DeleteUser()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        public UserDetailsDto GetOtherUserInformation(string userId)
        {
            var user = _dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();

            //TODO evaluation
            return _userConverter.ConvertToUserDetaiolsDto(user);
        }

        public UserDetailsDto GetUserInformation()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();

            //TODO evaluation
            return _userConverter.ConvertToUserDetaiolsDto(user);
        }

        public void UpdateUserInformation(UserDetailsDto user)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(new Guid(signedInUserId) != user.Id)
            {
                throw new UpdateNotAllowedException("User can't update another user's information.");
            }

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }
    }
}
