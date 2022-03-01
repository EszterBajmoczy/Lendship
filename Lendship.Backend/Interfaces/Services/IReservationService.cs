using Lendship.Backend.DTO;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IReservationService
    {
        IEnumerable<ReservationDto> GetReservations();

        void CreateReservation(ReservationDto reservation, int advertisementId);

        void UpdateReservation(ReservationDto reservation);

        IEnumerable<ReservationDto> GetReservationsForUser();

        void UpdateReservationState(int reservationId, string state);

        void AdmitReservation(int reservationId);
    }
}
