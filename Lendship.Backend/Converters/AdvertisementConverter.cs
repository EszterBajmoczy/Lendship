using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System.Linq;

namespace Lendship.Backend.Converters
{
    public class AdvertisementConverter : IAdvertisementConverter
    {
        private IUserConverter _userConverter;

        public AdvertisementConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public AdvertisementDto ConvertToDto(Advertisement ad)
        {
            return new AdvertisementDto
            {
                Id = ad.Id,
                User = ad.User != null ? _userConverter.ConvertToDto(ad.User) : null,
                Title = ad.Title,
                Price = ad.Price,
                Credit = ad.Credit,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                Location = ad.Location,
                ImageLocation = ad.ImageLocations?.FirstOrDefault()?.Location
            };
        }
    }
}
