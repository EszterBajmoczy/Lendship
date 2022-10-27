using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
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
        private readonly IClosedGroupRepository _closedGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUsersAndClosedGroupsRepository _usersAndClosedGroupsRepository;

        private readonly IClosedGroupConverter _closedGroupConverter;

        public ClosedGroupService(
            IHttpContextAccessor httpContextAccessor, 
            IClosedGroupRepository closedGroupRepository,
            IUserRepository userRepository,
            IUsersAndClosedGroupsRepository usersAndClosedGroupsRepository,
            IClosedGroupConverter closedGroupConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _closedGroupRepository = closedGroupRepository;
            _userRepository = userRepository;
            _usersAndClosedGroupsRepository = usersAndClosedGroupsRepository;

            _closedGroupConverter = closedGroupConverter;
        }

        public ClosedGroupDto GetClosedGroupOfAdvertisement(int advertisementId)
        {
            var usersAndCloseGroups = _usersAndClosedGroupsRepository.GetByAdvertisement(advertisementId);
            return _closedGroupConverter.ConvertToDto(advertisementId, usersAndCloseGroups);
        }

        public void CreateClosedGroup(ClosedGroupDto closedGroup)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userIds = _userRepository.GetIdsByEmails(closedGroup.UserEmails);

            if (!userIds.Contains(signedInUserId))
            {
                userIds.Append(signedInUserId);
            }

            var cGroup = _closedGroupConverter.ConvertToEntity(closedGroup);
            _closedGroupRepository.Create(cGroup);

            _usersAndClosedGroupsRepository.Create(cGroup.Id, userIds);
        }

        public void AddUserToClosedGroup(string email, int closedGroupId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetByEmail(email);
            var isModificationAllowed = _usersAndClosedGroupsRepository.IsModificationAllowed(closedGroupId, signedInUserId);

            if(!isModificationAllowed)
            {
                throw new InvalidOperationException("Modification not allowed.");
            } else if (user == null)
            {
                throw new UserNotFoundException("User with the email address not found: " + email);
            }

            _usersAndClosedGroupsRepository.Create(closedGroupId, user.Id);
        }

        public void RemoveUserToClosedGroup(string email, int closedGroupId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetByEmail(email);
            var isModificationAllowed = _usersAndClosedGroupsRepository.IsModificationAllowed(closedGroupId, signedInUserId);

            if (!isModificationAllowed)
            {
                throw new InvalidOperationException("Modification not allowed.");
            }
            else if (user == null)
            {
                throw new UserNotFoundException("User with the email address not found: " + email);
            }

            _usersAndClosedGroupsRepository.Delete(closedGroupId, user.Id);
        }
    }
}
