using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IUserConverter
    {
        UserDto ConvertToDto(ApplicationUser user);

        UserDetailsDto ConvertToUserDetailsDto(ApplicationUser user);
    }
}
