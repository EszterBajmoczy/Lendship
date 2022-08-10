using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class ClosedGroupService : IClosedGroupService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly IUserConverter _userConverter;
        private readonly IClosedGroupConverter _cgConverter;

        public ClosedGroupService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            _userConverter = new UserConverter();
            _cgConverter = new ClosedGroupConverter(_userConverter);
        }

        public ClosedGroupDto GetClosedGroupOfAdvertisement(int advertisementId)
        {
            var usersAndCloseGroups = _dbContext.UsersAndClosedGroups
                                .Include(u => u.User)
                                .Where(x => x.ClosedGroup.AdvertismentId == advertisementId)
                                .ToList();

            var closedGroupDto = _cgConverter.ConvertToDto(advertisementId, usersAndCloseGroups);
            return closedGroupDto;
        }

        public void CreateClosedGroup(ClosedGroupDto closedGroup)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userIds = _dbContext.Users
                        .Where(u => closedGroup.UserEmails.Contains(u.Email))
                        .Select(u => u.Id)
                        .ToList();

            if (!userIds.Contains(signedInUserId))
            {
                userIds.Add(signedInUserId);
            }

            var cGroup = _cgConverter.ConvertToEntity(closedGroup);
            _dbContext.ClosedGroups.Add(cGroup);
            _dbContext.SaveChanges();

            foreach (var userId in userIds)
            {
                var relation = new UsersAndClosedGroups()
                {
                    UserId = userId,
                    ClosedGroupId = cGroup.Id
                };
                _dbContext.UsersAndClosedGroups.Add(relation);
            }

            _dbContext.SaveChanges();
        }

        public void AddUserToClosedGroup(string email, int closedGroupId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefault();
            var validModification = _dbContext.UsersAndClosedGroups
                                .Any(x => x.ClosedGroupId == closedGroupId && x.UserId == signedInUserId);

            if(!validModification)
            {
                throw new InvalidOperationException("Modification not allowed.");
            } else if (user == null)
            {
                throw new UserNotFoundException("User with the email address not found: " + email);
            }

            var newRelation = new UsersAndClosedGroups()
            {
                UserId = user.Id,
                ClosedGroupId = closedGroupId
            };

            _dbContext.UsersAndClosedGroups.Add(newRelation);
            _dbContext.SaveChanges();
        }

        public void RemoveUserToClosedGroup(string email, int closedGroupId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users
                .Where(x => x.Email == email)
                .FirstOrDefault();
            var validModification = _dbContext.UsersAndClosedGroups
                                .Any(x => x.ClosedGroupId == closedGroupId && x.UserId == signedInUserId);

            var entity = _dbContext.UsersAndClosedGroups
                                    .Where(u => u.ClosedGroupId == closedGroupId && u.UserId == user.Id)
                                    .FirstOrDefault();

            if (!validModification)
            {
                throw new InvalidOperationException("Modification not allowed.");
            }
            else if (user == null)
            {
                throw new UserNotFoundException("User with the email address not found: " + email);
            }

            _dbContext.UsersAndClosedGroups.Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
