using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IPrivateUserRepository
    {
        IEnumerable<PrivateUser> GetByAdvertisement(int advertisementId);

        void CreateAll(IEnumerable<PrivateUser> privateUsers);

        void DeleteAll(IEnumerable<PrivateUser> privateUsers);
    }
}