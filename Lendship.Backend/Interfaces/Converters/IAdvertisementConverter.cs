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
        AdvertisementDto ConvertToDto(Advertisement ad);
    }
}
