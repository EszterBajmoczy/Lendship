using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;

namespace Lendship.Backend.Repositories
{
    public class ClosedGroupRepository : IClosedGroupRepository
    {
        private readonly LendshipDbContext _dbContext;

        public ClosedGroupRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(ClosedGroup closedGroup)
        {
            _dbContext.ClosedGroups.Add(closedGroup);
            _dbContext.SaveChanges();
        }
    }
}
