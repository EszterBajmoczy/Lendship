using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;

namespace Lendship.Backend.Converters
{
    public class PrivateUserConverter : IPrivateUserConverter
    {
        private IUserConverter _userConverter;
        public PrivateUserConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public PrivateUser ConvertToEntity(int advertisementId, UserDto user)
        {
            return new PrivateUser()
            {
                AdvertisementId = advertisementId,
                UserId = user.Id.ToString(),
                UserEmail = user.Email
            };
        }
    }
}
