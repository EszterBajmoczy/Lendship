using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IAdvertisementConverter
    {
        AdvertisementDto ConvertToDto(Advertisement ad);

        AdvertisementDetailsDto ConvertToDetailsDto(Advertisement ad);

        Advertisement ConvertToEntity(AdvertisementDetailsDto ad, ApplicationUser user, Category category);
    }
}
