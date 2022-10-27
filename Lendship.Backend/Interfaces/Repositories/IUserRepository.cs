using Lendship.Backend.Authentication;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IUserRepository
    {
        ApplicationUser GetById(string id);

        ApplicationUser GetByEmail(string email);

        IEnumerable<string> GetIdsByEmails(List<string> emails);

        void Create(ApplicationUser user);

        void Update(ApplicationUser user);

        void DeleteById(string userId);
    }
}