using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly LendshipDbContext _dbContext;

        public AdvertisementRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Advertisement> GetAll(string signedInUserId)
        {
            return _dbContext.Advertisements
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.ImageLocations)
                .Include(a => a.Availabilities)
                .Include(a => a.PrivateUsers)
                .ThenInclude(p => p.User)
                .Where(x => x.IsPublic || x.User.Id == signedInUserId || x.PrivateUsers.Any(p => p.UserId == signedInUserId));
        }

        public Advertisement GetById(int? id, string signedInUserId)
        {
            return _dbContext.Advertisements
                .AsNoTracking()
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.ImageLocations)
                .Include(a => a.Availabilities)
                .Include(a => a.PrivateUsers)
                .ThenInclude(p => p.User)
                .Where(a => a.Id == id && (a.IsPublic || a.User.Id == signedInUserId || a.PrivateUsers.Any(p => p.UserId == signedInUserId)))
                .FirstOrDefault();
        }

        public void Create(Advertisement ad)
        {
            _dbContext.Advertisements.Add(ad);
            _dbContext.SaveChanges();
        }

        public void Update(Advertisement ad)
        {
            _dbContext.Advertisements.Update(ad);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var advertisement = _dbContext.Advertisements
                .Where(a => a.Id == id)
                .FirstOrDefault();

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found");
            }

            _dbContext.Advertisements.Remove(advertisement);
            _dbContext.SaveChanges();
        }
    }
}
