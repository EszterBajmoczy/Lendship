using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IAvailabilityConverter
    {
        AvailabilityDto ConvertToDto(Availability availability);

        Availability ConvertToEntity(AvailabilityDto availability, Advertisement advertisement);
    }
}
