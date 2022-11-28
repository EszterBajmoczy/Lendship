using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IPrivateUserService
    {
        void UpdatePrivateUsers(int advertisementId, List<UserDto> privateUsers);

        IEnumerable<PrivateUser> GetByAdvertisement(int advertisementId);
    }
}
