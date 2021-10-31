using Lendship.Backend.DTO;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IClosedGroupService
    {
        void CreateClosedGroup(ClosedGroupDto closedGroup);

        void UpdateClosedGroup(ClosedGroupDto closedGroup);

        ClosedGroupDto GetClosedGroupOfAdvertisement(int advertisementId);
    }
}
