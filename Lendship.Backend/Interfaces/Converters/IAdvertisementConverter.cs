using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IAdvertisementConverter
    {
        Advertisement ConvertToEntity(AdvertisementDto ad, ApplicationUser user, Category category);

        AdvertisementDto ConvertToDto(Advertisement ad, IEnumerable<Availability> availabilities);

    }
}
