using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class UsersAndClosedGroupsRepository : IUsersAndClosedGroupsRepository
    {
        private readonly LendshipDbContext _dbContext;

        public UsersAndClosedGroupsRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(int closedGroupId, IEnumerable<string> userIds)
        {
            var toAdd = userIds.Select(x => new UsersAndClosedGroups()
                        {
                            UserId = x,
                            ClosedGroupId = closedGroupId
                        });

            _dbContext.UsersAndClosedGroups.AddRange(toAdd);
            _dbContext.SaveChanges();
        }

        public void Create(int closedGroupId, string userId)
        {
            var toAdd = new UsersAndClosedGroups()
            {
                UserId = userId,
                ClosedGroupId = closedGroupId
            };

            _dbContext.UsersAndClosedGroups.Add(toAdd);
            _dbContext.SaveChanges();
        }

        public void Delete(int closedGroupId, string userId)
        {
            var toDelete = _dbContext.UsersAndClosedGroups
                                .Where(u => u.ClosedGroupId == closedGroupId && u.UserId == userId)
                                .FirstOrDefault();

            _dbContext.UsersAndClosedGroups.Remove(toDelete);
            _dbContext.SaveChanges();
        }

        public IEnumerable<UsersAndClosedGroups> GetByAdvertisement(int advertisementId)
        {
            return _dbContext.UsersAndClosedGroups
                        .Include(u => u.User)
                        .Where(x => x.ClosedGroup.AdvertismentId == advertisementId);
        }

        public bool IsModificationAllowed(int closedGroupId, string userId)
        {
            return _dbContext.UsersAndClosedGroups
                        .Any(x => x.ClosedGroupId == closedGroupId && x.UserId == userId);
        }
    }
}
