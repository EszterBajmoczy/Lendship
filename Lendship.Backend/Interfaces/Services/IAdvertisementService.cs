using Lendship.Backend.DTO;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IAdvertisementService
    {
        AdvertisementDetailsDto GetAdvertisement(int advertisementId);

        int CreateAdvertisement(AdvertisementDetailsDto advertisement);

        void UpdateAdvertisement(AdvertisementDetailsDto advertisement);

        void DeleteAdvertisement(int advertisementId);

        void RemoveSavedAdvertisement(int advertisementId);

        void SaveAdvertisementForUser(int advertisementId);

        bool IsAdvertisementSaved(int advertisementId);

        AdvertisementListDto GetAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy, int page);

        AdvertisementListDto GetUsersAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy, int page);

        AdvertisementListDto GetSavedAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy, int page);

    }
}
