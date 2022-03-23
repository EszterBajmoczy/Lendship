using Lendship.Backend.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IAdvertisementService
    {
        AdvertisementDetailsDto GetAdvertisement(int advertisementId);

        void CreateAdvertisement(AdvertisementDetailsDto advertisement);

        void UpdateAdvertisement(AdvertisementDetailsDto advertisement);

        void DeleteAdvertisement(int advertisementId);

        void RemoveSavedAdvertisement(int advertisementId);

        void SaveAdvertisementForUser(int advertisementId);

        IEnumerable<AdvertisementDto> GetAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy);

        IEnumerable<AdvertisementDto> GetUsersAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy);

        IEnumerable<AdvertisementDto> GetSavedAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy);

    }
}
