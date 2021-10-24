using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IReservationConverter
    {
        Reservation ConvertToEntity(ReservationDto reservationDto, ApplicationUser user, Advertisement advertisement);

        ReservationDto ConvertToDto(Reservation reservation);

    }
}
