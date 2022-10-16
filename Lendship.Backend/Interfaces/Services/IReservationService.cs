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

        bool IsReservationClosed(int reservationId);

        IEnumerable<ReservationDto> GetReservationsForAdvertisement(int advertisementId);

        IEnumerable<ReservationForAdvertisementDto> GetRecentReservations();

        ReservationTokenDto GetReservationToken(int reservationId, bool closing);

        TransactionOperationDto ValidateReservationToken(string reservationToken);

    }
}
