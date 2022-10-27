using Lendship.Backend.Authentication;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface ISavedAdvertisementRepository
    {
        IEnumerable<int> GetSavedAdvertisementIdsByUser(string userId);

        void DeleteAll(string userId, int advertisementId);

        SavedAdvertisement Get(string userId, int advertisementId);

        void Create(SavedAdvertisement savedAd);
    }
}