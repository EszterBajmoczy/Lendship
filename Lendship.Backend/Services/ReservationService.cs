using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly IReservationConverter _reservationConverter;

        public ReservationService(INotificationService notificationService, IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            _reservationConverter = new ReservationConverter(new AdvertisementConverter());
        }
                
        public void CreateReservation(ReservationDto reservationDto, int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();
            var advertisement = _dbContext.Advertisements.Where(x => x.Id == advertisementId).FirstOrDefault();

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not exists.");
            }

            var reservation = _reservationConverter.ConvertToEntity(reservationDto, user, advertisement);

            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ReservationDto> GetReservations()
        {
            var resultList = new List<ReservationDto>();

            var reservations = _dbContext.Reservations
                        .Include(r => r.Advertisement)
                        .Include(a => a.Advertisement.ImageLocations)
                        .ToList();

            foreach (var res in reservations)
            {
                var dto = _reservationConverter.ConvertToDto(res);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<ReservationDto> GetReservationsForUser()
        {
            var resultList = new List<ReservationDto>();

            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _dbContext.Reservations
                                    .Include(r => r.Advertisement)
                                    .Include(r => r.Advertisement.User)
                                    .Where(r => r.Advertisement.User.Id == signedInUserId)
                                    .ToList();

            foreach (var res in reservations)
            {
                var dto = _reservationConverter.ConvertToDto(res);
                resultList.Add(dto);
            }

            return resultList;
        }

        public void UpdateReservation(ReservationDto reservationDto)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var oldRes = _dbContext.Reservations
                            .AsNoTracking()
                            .Include(r => r.User)
                            .Include(r => r.Advertisement)
                            .Where(r => r.Id == reservationDto.Id)
                            .FirstOrDefault();
            if (oldRes == null)
            {
                throw new ReservationNotFoundException("Reservation not found.");
            }

            if (oldRes.Advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            if(signedInUserId != oldRes.User.Id)
            {
                throw new UpdateNotAllowedException("Update not allowed.");
            }

            var reservation = _reservationConverter.ConvertToEntity(reservationDto, oldRes.User, oldRes.Advertisement);

            _dbContext.Update(reservation);
            _dbContext.SaveChanges();
        }

        public void AdmitReservation(int reservationId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = _dbContext.Reservations
                                .AsNoTracking()
                                .Include(r => r.User)
                                .Include(r => r.Advertisement)
                                .Where(r => r.Id == reservationId)
                                .FirstOrDefault();

            if (reservation == null)
            {
                throw new ReservationNotFoundException("Reservation not found.");
            }

            if (reservation.Advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            if (signedInUserId == reservation.User.Id)
            {
                reservation.admittedByLender = true;
                _notificationService.CreateNotification("Reservation was admitted by the Lender", reservation, reservation.Advertisement.User.Id);
            }

            if (signedInUserId == reservation.Advertisement.User.Id)
            {
                reservation.admittedByAdvertiser = true;
                _notificationService.CreateNotification("Reservation was admitted by the Advertiser", reservation, reservation.User.Id);
            }

            if (reservation.admittedByAdvertiser && reservation.admittedByLender)
            {
                reservation.ReservationState = ReservationState.Closed;
            }

            _dbContext.Update(reservation);
            _dbContext.SaveChanges();
        }

        public void UpdateReservationState(int reservationId, string state)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = _dbContext.Reservations
                                .AsNoTracking()
                                .Include(r => r.User)
                                .Include(r => r.Advertisement)
                                .Where(r => r.Id == reservationId)
                                .FirstOrDefault();

            if (reservation == null)
            {
                throw new ReservationNotFoundException("Reservation not found.");
            }

            if (reservation.Advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            if (signedInUserId != reservation.User.Id && signedInUserId != reservation.Advertisement.User.Id)
            {
                throw new UpdateNotAllowedException("Update not allowed.");
            }

            var reservationState = GetReservationState(state);

            if (reservationState == ReservationState.Closed)
            {
                throw new UpdateNotAllowedException("Can not set Closed state this way.");
            }

            _notificationService.CreateNotification("Reservation state changed: " + reservationState, reservation, signedInUserId == reservation.User.Id ? signedInUserId : reservation.Advertisement.User.Id);
            
            reservation.ReservationState = reservationState;

            _dbContext.Update(reservation);
            _dbContext.SaveChanges();
        }

        private ReservationState GetReservationState(string state)
        {
            switch (state)
            {
                case "Created":
                    return ReservationState.Created;
                case "Accepted":
                    return ReservationState.Accepted;
                case "Declined":
                    return ReservationState.Declined;
                case "Resigned":
                    return ReservationState.Resigned;
                case "Closed":
                    return ReservationState.Closed;
                default:
                    return ReservationState.Created;
            }
        }
    }
}
