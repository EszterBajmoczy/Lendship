using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IAdvertisementConverter
    {
        AdvertisementDto ConvertToDto(Advertisement ad);
    }
}
