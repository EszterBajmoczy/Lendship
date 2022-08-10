using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IClosedGroupConverter
    {
        ClosedGroupDto ConvertToDto(int advertisementId, IEnumerable<UsersAndClosedGroups> usersAndClosedGroups);

        ClosedGroup ConvertToEntity(ClosedGroupDto closedGroupDto);
    }
}
