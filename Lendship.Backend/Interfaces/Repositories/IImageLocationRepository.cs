using Lendship.Backend.Authentication;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IImageLocationRepository
    {
        void DeleteImageFromAdvertisement(int advertisementId, string fileName);

        void DeleteImagesByAdvertisement(int? advertisementId);

        void Create(int advertisementId, string location);
    }
}