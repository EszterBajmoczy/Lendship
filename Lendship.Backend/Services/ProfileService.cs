using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IUserConverter _userConverter;

        public ProfileService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IUserConverter userConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;

            _userConverter = userConverter;
        }

        public void DeleteUser()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _userRepository.DeleteById(signedInUserId);
        }

        public UserDetailsDto GetOtherUserInformation(string userId)
        {
            var user = _userRepository.GetById(userId);

            //TODO evaluation
            return _userConverter.ConvertToUserDetailsDto(user);
        }

        public UserDetailsDto GetUserInformation()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetById(signedInUserId);

            //TODO evaluation
            return _userConverter.ConvertToUserDetailsDto(user);
        }

        public void UpdateUserInformation(UserDetailsDto userDto)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(new Guid(signedInUserId) != userDto.Id)
            {
                throw new UpdateNotAllowedException("User can't update another user's information.");
            }
            var user = _userRepository.GetById(signedInUserId);
            _userRepository.Update(user);
        }
    }
}
