using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly LendshipDbContext _dbContext;

        public AvailabilityRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Availability> GetByAdvertisement(int advertisementId)
        {
            return _dbContext.Availabilites
                        .Where(a => a.AdvertisementId == advertisementId);
        }

        public void DeleteRange(IEnumerable<Availability> availabilities)
        {
            _dbContext.Availabilites.RemoveRange(availabilities);
            _dbContext.SaveChanges();
        }

        public void AddRange(IEnumerable<Availability> availabilities)
        {
            _dbContext.Availabilites.AddRange(availabilities);
            _dbContext.SaveChanges();
        }
    }
}
