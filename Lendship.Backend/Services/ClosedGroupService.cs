using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var cGroup = _dbContext.ClosedGroups
                .Where(c => c.AdvertismentId == advertisementId)
                .FirstOrDefault();
            var userIds = cGroup.UserIds.Select(cg => cg.ToString());

            var users = _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToList();

            var closedGroupDto = _cgConverter.ConvertToDto(cGroup, users);
            return closedGroupDto;
        }

        public void CreateClosedGroup(ClosedGroupDto closedGroup)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guid = new Guid(signedInUserId);

            var userIds = closedGroup
                             .Users
                             .Where(x => x.Id.HasValue)
                             .Select(x => x.Id.GetValueOrDefault())
                             .ToList();

            if (!userIds.Contains(guid))
            {
                userIds.Add(guid);
            }

            var cGroup = _cgConverter.ConvertToEntity(closedGroup, userIds);

            _dbContext.ClosedGroups.Add(cGroup);
            _dbContext.SaveChanges();
        }

        public void UpdateClosedGroup(ClosedGroupDto closedGroup)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guid = new Guid(signedInUserId);

            var userIds = closedGroup
                             .Users
                             .Where(x => x.Id.HasValue)
                             .Select(x => x.Id.GetValueOrDefault())
                             .ToList();

            if (!userIds.Contains(guid))
            {
                userIds.Add(guid);
            }

            var cGroup = _cgConverter.ConvertToEntity(closedGroup, userIds);

            _dbContext.Update(cGroup);
            _dbContext.SaveChanges();
        }
    }
}
