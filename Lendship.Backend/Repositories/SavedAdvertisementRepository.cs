using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class SavedAdvertisementRepository : ISavedAdvertisementRepository
    {
        private readonly LendshipDbContext _dbContext;

        public SavedAdvertisementRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<int> GetSavedAdvertisementIdsByUser(string userId)
        {
            return _dbContext.SavedAdvertisements
                    .Where(sa => sa.UserId == userId)
                    .Select(a => a.AdvertisementId);
        }

        public void DeleteAll(string userId, int advertisementId)
        {
            var savedAds = _dbContext.SavedAdvertisements
                .Where(sa => sa.UserId == userId && sa.AdvertisementId == advertisementId);
            _dbContext.SavedAdvertisements.RemoveRange(savedAds);
            _dbContext.SaveChanges();
        }

        public SavedAdvertisement Get(string userId, int advertisementId)
        {
            return _dbContext.SavedAdvertisements
                .Where(sa => sa.UserId == userId && sa.AdvertisementId == advertisementId)
                .FirstOrDefault();
        }

        public void Create(SavedAdvertisement savedAd)
        {
            _dbContext.SavedAdvertisements.Add(savedAd);
            _dbContext.SaveChanges();
        }
    }
}
