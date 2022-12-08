using Lendship.Backend.DTO;
using Lendship.Backend.Models;
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

        void RemoveUpcommingReservations(int advertisementId);

        void RemoveUpcommingReservationForAvailabilities(int advertisementId, IEnumerable<Availability> availabilities);

        IEnumerable<ReservationForAdvertisementDto> GetRecentReservations();

        ReservationTokenDto GetReservationToken(int reservationId, bool closing);

        TransactionOperationDto ValidateReservationToken(string reservationToken);

    }
}
