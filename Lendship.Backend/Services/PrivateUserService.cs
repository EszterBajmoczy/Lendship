using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Lendship.Backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class PrivateUserService : IPrivateUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IPrivateUserRepository _privateUserRepository;

        private readonly IPrivateUserConverter _privateUserConverter;

        public PrivateUserService(
            IHttpContextAccessor httpContextAccessor, 
            IUserRepository userRepository,
            IPrivateUserRepository privateUserRepository,
            IPrivateUserConverter privateUserConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _privateUserRepository = privateUserRepository;

            _privateUserConverter = privateUserConverter;
        }

        public void UpdatePrivateUsers(int advertisementId, List<UserDto> privateUsers)
        {
            var users = _privateUserRepository.GetByAdvertisement(advertisementId);

            var toRemoveIds = users.Where(x => !privateUsers.Any(p => p.Id.ToString() == x.UserId));
            var toAdd = privateUsers
                            .Where(x => !users.Any(p => p.UserId == x.Id.ToString()))
                            .Select(x => _privateUserConverter.ConvertToEntity(advertisementId, x));

            _privateUserRepository.DeleteAll(toRemoveIds);
            _privateUserRepository.CreateAll(toAdd);
        }

        public IEnumerable<PrivateUser> GetByAdvertisement(int advertisementId)
        {
            return _privateUserRepository.GetByAdvertisement(advertisementId);
        }
    }
}
