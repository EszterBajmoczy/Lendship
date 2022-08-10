using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IAdvertisementDetailsConverter
    {
        Advertisement ConvertToEntity(AdvertisementDetailsDto ad, ApplicationUser user, Category category);

        AdvertisementDetailsDto ConvertToDto(Advertisement ad);

    }
}
