using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IClosedGroupRepository
    {
        void Create(ClosedGroup closedGroup);
    }
}