using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Converters
{
    public class AdvertisementConverter : IAdvertisementConverter
    {
        public AdvertisementDto ConvertToDto(Advertisement ad)
        {
            return new AdvertisementDto
            {
                Id = ad.Id,
                Title = ad.Title,
                Price = ad.Price,
                Credit = ad.Credit,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude
            };
        }
    }
}
