using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class ImageLocationRepository : IImageLocationRepository
    {
        private readonly LendshipDbContext _dbContext;

        public ImageLocationRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(int advertisementId, string location)
        {
            var imageLocation = new ImageLocation()
            {
                AdvertisementId = advertisementId,
                Location = location
            };

            _dbContext.ImageLocations.Add(imageLocation);
            _dbContext.SaveChanges();
        }

        public void DeleteImageFromAdvertisement(int advertisementId, string fileName)
        {
            var images = _dbContext.ImageLocations.Where(i => i.AdvertisementId == advertisementId && i.Location.Contains(fileName));
            _dbContext.RemoveRange(images);
            _dbContext.SaveChanges();
        }

        public void DeleteImagesByAdvertisement(int? advertisementId)
        {
            var images = _dbContext.ImageLocations.Where(i => i.AdvertisementId == advertisementId);
            _dbContext.RemoveRange(images);
            _dbContext.SaveChanges();
        }
    }
}
