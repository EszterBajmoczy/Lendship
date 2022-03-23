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
            var userDtos = new List<string>();

            foreach (var user in users)
            {
                userDtos.Add(user.Email);
            }

            return new ClosedGroupDto()
            {
                Id = closedGroup.Id,
                AdvertisementId = closedGroup.AdvertismentId,
                UserEmails = userDtos
            };
        }

        public ClosedGroup ConvertToEntity(ClosedGroupDto closedGroupDto)
        {
            return new ClosedGroup()
            {
                Id = closedGroupDto.Id,
                AdvertismentId = closedGroupDto.AdvertisementId
            };
        }
    }
}
