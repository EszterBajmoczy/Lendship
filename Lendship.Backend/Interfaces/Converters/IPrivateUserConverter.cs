using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IPrivateUserConverter
    {
        PrivateUser ConvertToEntity(int advertisementId, UserDto usersAndAdvertisement);
    }
}
