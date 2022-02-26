using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Converters
{
    public class ClosedGroupConverter : IClosedGroupConverter
    {
        private IUserConverter _userConverter;
        public ClosedGroupConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public ClosedGroupDto ConvertToDto(ClosedGroup closedGroup, List<ApplicationUser> users)
        {
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                //TODO evaulation
                var userDto = _userConverter.ConvertToDto(user);
                userDtos.Add(userDto);
            }

            return new ClosedGroupDto()
            {
                Id = closedGroup.Id,
                AdvertisementId = closedGroup.AdvertismentId,
                Users = userDtos
            };
        }

        public ClosedGroup ConvertToEntity(ClosedGroupDto closedGroupDto, List<Guid> userIds)
        {
            return new ClosedGroup()
            {
                Id = closedGroupDto.Id ?? 0,
                AdvertismentId = closedGroupDto.AdvertisementId
            };
        }
    }
}
