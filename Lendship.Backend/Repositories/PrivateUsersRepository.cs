using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class PrivateUsersRepository : IPrivateUserRepository
    {
        private readonly LendshipDbContext _dbContext;

        public PrivateUsersRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateAll(IEnumerable<PrivateUser> privateUsers)
        {
            _dbContext.PrivateUsers.AddRange(privateUsers);
            _dbContext.SaveChanges();
        }

        public void DeleteAll(IEnumerable<PrivateUser> privateUsers)
        {
            _dbContext.PrivateUsers.RemoveRange(privateUsers);
            _dbContext.SaveChanges();
        }

        public IEnumerable<PrivateUser> GetByAdvertisement(int advertisementId)
        {
            return _dbContext.PrivateUsers
                        .Include(u => u.User)
                        .Where(x => x.AdvertisementId == advertisementId);
        }
    }
}
