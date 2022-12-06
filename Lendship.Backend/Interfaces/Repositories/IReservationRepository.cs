using Lendship.Backend.Authentication;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Reservation GetById(int? id);

        void RemoveUpcommingReservations(int advertisementId);

        IEnumerable<Reservation> GetByUser(string userId);

        IEnumerable<Reservation> GetForUserAdvertisement(string userId);

        IEnumerable<Reservation> GetByAdvertisement(int advertisementId);

        IEnumerable<Reservation> GetRecentReservations(string userId);

        Reservation GetReservation(string userId, string signedInUserId, int reservationId);

        bool IsReservationClosed(int reservationId);

        void Create(Reservation reservation);

        void Update(Reservation reservation);

        void Delete(Reservation reservation);
    }
}