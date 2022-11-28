using Lendship.Backend.Authentication;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IAvailabilityRepository
    {
        IEnumerable<Availability> GetByAdvertisement(int advertisementId);

        void DeleteRange(IEnumerable<Availability> availabilities);

        void AddRange(IEnumerable<Availability> availabilities);
    }
}