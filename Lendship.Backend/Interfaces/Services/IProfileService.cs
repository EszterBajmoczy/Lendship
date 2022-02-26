using Lendship.Backend.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IProfileService
    {
        UserDetailsDto GetUserInformation();

        UserDetailsDto GetOtherUserInformation(string userId);

        void UpdateUserInformation(UserDetailsDto user);

        void DeleteUser();
    }
}
