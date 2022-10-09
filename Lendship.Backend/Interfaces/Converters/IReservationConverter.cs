using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IReservationConverter
    {
        Reservation ConvertToEntity(ReservationDetailDto reservationDto, ApplicationUser user, Advertisement advertisement);

        ReservationDetailDto ConvertToDetailDto(Reservation reservation);

        ReservationDto ConvertToDto(Reservation reservation);

        ReservationForAdvertisementDto ConvertToReservationForAdvertisementDto(Reservation reservation);
    }
}
