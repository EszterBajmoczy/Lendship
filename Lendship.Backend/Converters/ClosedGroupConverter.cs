using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Converters
{
    public class ClosedGroupConverter : IClosedGroupConverter
    {
        private IUserConverter _userConverter;
        public ClosedGroupConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public ClosedGroupDto ConvertToDto(int advertisementId, IEnumerable<UsersAndClosedGroups> usersAndClosedGroups)
        {
            return new ClosedGroupDto()
            {
                AdvertisementId = advertisementId,
                UserEmails = usersAndClosedGroups.Select(u => u.User.Email).ToList()
            };
        }

        public ClosedGroup ConvertToEntity(ClosedGroupDto closedGroupDto)
        {
            return new ClosedGroup()
            {
                AdvertismentId = closedGroupDto.AdvertisementId
            };
        }
    }
}
