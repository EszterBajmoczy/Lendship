using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly LendshipDbContext _dbContext;

        public ReservationRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Reservation GetById(int? id)
        {
            return _dbContext.Reservations
                            .AsNoTracking()
                            .Include(r => r.User)
                            .Include(r => r.User.Evaluation)
                            .Include(r => r.Advertisement)
                            .Include(r => r.Advertisement.User)
                            .Include(r => r.Advertisement.User.Evaluation)
                            .Where(r => r.Id == id)
                            .FirstOrDefault();
        }

        public void RemoveUpcommingReservations(int advertisementId)
        {
            var reservations = _dbContext.Reservations
                .Where(r => r.Advertisement.Id == advertisementId && r.DateFrom >= DateTime.Now)
                .Include(r => r.User)
                .Include(r => r.User.Evaluation)
                .ToList();
            _dbContext.Reservations.RemoveRange(reservations);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Reservation> GetByUser(string userId)
        {
            return _dbContext.Reservations
                        .Include(r => r.Advertisement)
                        .Include(r => r.Advertisement.User)
                        .Include(r => r.Advertisement.User.Evaluation)
                        .Include(a => a.Advertisement.ImageLocations)
                        .Include(r => r.User)
                        .Include(r => r.User.Evaluation)
                        .Where(r => r.User.Id == userId);
        }

        public IEnumerable<Reservation> GetForUserAdvertisement(string userId)
        {
            return _dbContext.Reservations
                    .Include(r => r.Advertisement)
                    .Include(r => r.Advertisement.User)
                    .Include(r => r.Advertisement.User.Evaluation)
                    .Include(a => a.Advertisement.ImageLocations)
                    .Include(r => r.User)
                    .Include(r => r.User.Evaluation)
                    .Where(r => r.Advertisement.User.Id == userId);
        }

        public IEnumerable<Reservation> GetByAdvertisement(int advertisementId)
        {
            return _dbContext.Reservations
                    .Include(r => r.User)
                    .Include(r => r.User.Evaluation)
                    .Include(r => r.Advertisement)
                    .Include(r => r.Advertisement.User)
                    .Include(r => r.Advertisement.User.Evaluation)
                    .Where(r => r.Advertisement.Id == advertisementId);
        }

        public IEnumerable<Reservation> GetRecentReservations(string userId)
        {
            var begin = DateTime.UtcNow.AddDays(-5);
            var end = DateTime.UtcNow.AddDays(5);

            return _dbContext.Reservations
                    .Include(r => r.Advertisement)
                    .Include(r => r.Advertisement.User)
                    .Include(r => r.Advertisement.User.Evaluation)
                    .Where(r => (r.Advertisement.User.Id == userId || r.User.Id == userId) && r.ReservationState != ReservationState.Closed
                            && ((r.DateFrom > begin && r.DateFrom < end) || (r.DateTo > begin && r.DateTo < end)));
        }

        public Reservation GetReservation(string userId, string signedInUserId, int reservationId)
        {
            return _dbContext.Reservations
                                .AsNoTracking()
                                .Include(r => r.User)
                                .Include(r => r.User.Evaluation)
                                .Include(r => r.User.ReservedCredits)
                                .Include(r => r.Advertisement)
                                .Include(r => r.Advertisement.User)
                                .Include(r => r.Advertisement.User.Evaluation)
                                .Where(r => r.Id == reservationId
                                    && (r.User.Id == userId || r.Advertisement.User.Id == userId)
                                    && (r.User.Id == signedInUserId || r.Advertisement.User.Id == signedInUserId))
                                .FirstOrDefault();
        }

        public bool IsReservationClosed(int reservationId)
        {
            return _dbContext.Reservations
                        .Where(r => r.Id == reservationId)
                        .Select(r => r.ReservationState == ReservationState.Closed)
                        .FirstOrDefault();
        }

        public void Create(Reservation reservation)
        {
            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public void Update(Reservation reservation)
        {
            _dbContext.Update(reservation);
            _dbContext.SaveChanges();
        }

        public void Delete(Reservation reservation)
        {
            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();
        }

        public void Delete(List<Reservation> reservations)
        {
            _dbContext.Reservations.RemoveRange(reservations);
            _dbContext.SaveChanges();
        }
    }
}
