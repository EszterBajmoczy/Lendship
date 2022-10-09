using Lendship.Backend.DTO;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IReservationService
    {
        IEnumerable<ReservationDetailDto> GetReservations();

        void CreateReservation(ReservationDetailDto reservation, int advertisementId);

        void UpdateReservation(ReservationDetailDto reservation);

        IEnumerable<ReservationDetailDto> GetReservationsForUser();

        void UpdateReservationState(int reservationId, string state);

        void AdmitReservation(int reservationId);

        IEnumerable<ReservationDto> GetReservationsForAdvertisement(int advertisementId);

        IEnumerable<ReservationForAdvertisementDto> GetRecentReservationsForUser();

        IEnumerable<ReservationForAdvertisementDto> GetUsersRecentReservations();

        string GetReservationToken(int reservationId);

        bool ValidateReservationToken(string reservationToken, bool closing);

    }
}
