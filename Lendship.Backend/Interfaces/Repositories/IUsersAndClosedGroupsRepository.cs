using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IUsersAndClosedGroupsRepository
    {
        IEnumerable<UsersAndClosedGroups> GetByAdvertisement(int advertisementId);

        void Create(int closedGroupId, IEnumerable<string> userIds);

        void Create(int closedGroupId, string userId);

        void Delete(int closedGroupId, string userId);

        bool IsModificationAllowed(int closedGroupId, string userId);
    }
}