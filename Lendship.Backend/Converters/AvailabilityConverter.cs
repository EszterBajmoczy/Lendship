using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Converters
{
    public class AvailabilityConverter : IAvailabilityConverter
    {
        public AvailabilityDto ConvertToDto(Availability availability)
        {
            return new AvailabilityDto
            {
                Id = availability.Id,
                DateFrom = availability.DateFrom,
                DateTo = availability.DateTo
            };
        }

        public Availability ConvertToEntity(Availability availability, Advertisement advertisement)
        {
            return new Availability
            {
                Id = availability.Id,
                Advertisement = advertisement,
                DateFrom = availability.DateFrom,
                DateTo = availability.DateTo
            };
        }
    }
}
