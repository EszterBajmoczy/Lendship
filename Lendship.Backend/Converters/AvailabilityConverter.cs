using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;

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

        public Availability ConvertToEntity(AvailabilityDto availability, Advertisement advertisement)
        {
            var result = new Availability
            {
                Id = availability.Id ?? 0,
                AdvertisementId = advertisement.Id
            };

            if(availability.DateFrom != null && availability.DateTo != null)
            {
                result.DateFrom = availability.DateFrom.Value;
                result.DateTo = availability.DateTo.Value;
            }

            return result;
        }
    }
}
