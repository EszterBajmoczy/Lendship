using Lendship.Backend.DTO;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IClosedGroupService
    {
        void CreateClosedGroup(ClosedGroupDto closedGroup);

        public void AddUserToClosedGroup(string userId, int closedGroupId);

        public void RemoveUserToClosedGroup(string userId, int closedGroupId);

        ClosedGroupDto GetClosedGroupOfAdvertisement(int advertisementId);
    }
}
