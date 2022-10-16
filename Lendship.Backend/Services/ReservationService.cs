using Lendship.Backend.Authentication;
using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Lendship.Backend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly IReservationConverter _reservationConverter;
        private readonly IUserConverter _userConverter;

        public ReservationService(INotificationService notificationService, IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            _reservationConverter = new ReservationConverter(new AdvertisementConverter(new UserConverter()), new UserConverter());
            _userConverter = new UserConverter();
        }
                
        public void CreateReservation(ReservationDetailDto reservationDto, int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();
            var advertisement = _dbContext.Advertisements.Where(x => x.Id == advertisementId).FirstOrDefault();

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not exists.");
            }

            if (signedInUserId == advertisement.User.Id)
            {
                throw new Exception("Can not make reservation for own advertisement.");
            }

            var reservation = _reservationConverter.ConvertToEntity(reservationDto, user, advertisement);

            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ReservationDetailDto> GetReservations()
        {
            var resultList = new List<ReservationDetailDto>();

            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _dbContext.Reservations
                        .Include(r => r.Advertisement)
                        .Include(r => r.Advertisement.User)
                        .Include(a => a.Advertisement.ImageLocations)
                        .Include(r => r.User)
                        .Where(r => r.User.Id == signedInUserId)
                        .ToList();

            foreach (var res in reservations)
            {
                var dto = _reservationConverter.ConvertToDetailDto(res);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<ReservationDetailDto> GetReservationsForUser()
        {
            var resultList = new List<ReservationDetailDto>();

            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _dbContext.Reservations
                                    .Include(r => r.Advertisement)
                                    .Include(r => r.Advertisement.User)
                                    .Include(a => a.Advertisement.ImageLocations)
                                    .Include(r => r.User)
                                    .Where(r => r.Advertisement.User.Id == signedInUserId)
                                    .ToList();

            foreach (var res in reservations)
            {
                var dto = _reservationConverter.ConvertToDetailDto(res);
                resultList.Add(dto);
            }

            return resultList;
        }

        public void UpdateReservation(ReservationDetailDto reservationDto)
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
                                .Include(r => r.Advertisement.User)
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
                                .Include(r => r.Advertisement.User)
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

            _notificationService.CreateNotification("Reservation state changed: " + reservationState, reservation, signedInUserId == reservation.User.Id ? signedInUserId : reservation.Advertisement.User.Id);
            
            reservation.ReservationState = reservationState;

            _dbContext.Update(reservation);
            _dbContext.SaveChanges();
        }

        private void UpdateReservationState(Reservation reservation, string state, string signedInUserId)
        {
            var reservationState = GetReservationState(state);

            _notificationService.CreateNotification("Reservation state changed: " + reservationState, reservation, signedInUserId == reservation.User.Id ? signedInUserId : reservation.Advertisement.User.Id);

            reservation.ReservationState = reservationState;

            _dbContext.Update(reservation);
        }

        public IEnumerable<ReservationDto> GetReservationsForAdvertisement(int advertisementId)
        {
            var resultList = new List<ReservationDto>();

            var reservations = _dbContext.Reservations
                                    .Include(r => r.Advertisement)
                                    .Where(r => r.Advertisement.Id == advertisementId)
                                    .ToList();

            foreach (var res in reservations)
            {
                var dto = _reservationConverter.ConvertToDto(res);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<ReservationForAdvertisementDto> GetRecentReservations()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var begin = DateTime.UtcNow.AddDays(-5);
            var end = DateTime.UtcNow.AddDays(5);

            var reservations = _dbContext.Reservations
                                .Include(r => r.Advertisement)
                                .Include(r => r.Advertisement.User)
                                .Where(r => (r.Advertisement.User.Id == signedInUserId || r.User.Id == signedInUserId) && r.ReservationState != ReservationState.Closed
                                        && ((r.DateFrom > begin && r.DateFrom < end) || (r.DateTo > begin && r.DateTo < end)))
                                .Select(r => _reservationConverter.ConvertToReservationForAdvertisementDto(r, true))
                                .ToList();


            return reservations;
        }

        public ReservationTokenDto GetReservationToken(int reservationId, bool closing)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = _dbContext.Reservations
                                .Include(r => r.User)
                                .Include(r => r.Advertisement)
                                .Include(r => r.Advertisement.User)
                                .Where(r => r.Id == reservationId)
                                .FirstOrDefault();

            if (reservation == null)
            {
                throw new ReservationNotFoundException("Reservation not found.");
            } else if (reservation.User.Id != signedInUserId && reservation.Advertisement.User.Id != signedInUserId)
            {
                throw new ReservationNotFoundException("Not authorized operation.");
            }

            var builder = new StringBuilder();
            if (closing)
            {
                builder.Append(1);
            } else
            {
                builder.Append(0);
            }
            builder.Append(reservation.Id);
            builder.Append('-');
            builder.Append(signedInUserId);
            builder.Append('-');
            builder.Append(DateTime.UtcNow.Ticks);

            var result = new ReservationTokenDto()
            {
                ReservationToken = builder.ToString(),
                OtherUser = _userConverter.ConvertToDto(reservation.User.Id == signedInUserId ? reservation.User : reservation.Advertisement.User),
                ReservationId = reservation.Id,
                AdvertisementId = reservation.Advertisement.Id,
                IsLender = reservation.User.Id == signedInUserId ? true : false
            };

            return result;
        }

        public TransactionOperationDto ValidateReservationToken(string reservationToken)
        {
            var result = new TransactionOperationDto() { Succeeded = false };
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var indexFirst = reservationToken.IndexOf('-');
            var indexLast = reservationToken.LastIndexOf('-');

            if (indexFirst == -1)
            {
                return result;
            }

            var closing = int.Parse(reservationToken.Substring(0, 1));
            var reservationId = int.Parse(reservationToken.Substring(1, indexFirst-1));
            var userId = reservationToken.Substring(indexFirst + 1, indexLast - indexFirst-1);
            var time = long.Parse(reservationToken.Substring(indexLast + 1 , reservationToken.Length - indexLast - 1));
            var nowInTicks = DateTime.UtcNow.Ticks;

            //TODO close-dok szűrése
            var reservation = _dbContext.Reservations
                                .AsNoTracking()
                                .Include(r => r.User)
                                .Include(r => r.Advertisement)
                                .Include(r => r.Advertisement.User)
                                .Where(r => r.Id == reservationId
                                    && (r.User.Id == userId || r.Advertisement.User.Id == userId)
                                    && (r.User.Id == signedInUserId || r.Advertisement.User.Id == signedInUserId))
                                .FirstOrDefault();

            //if (reservation == null || !(time >= DateTime.UtcNow.AddMinutes(-5).Ticks && time < DateTime.UtcNow.Ticks))
            if (reservation == null)
            {
                return result;
            }

            if (closing == 1)
            {
                TransferCredit(reservation.User, reservation.Advertisement.User, reservation.Advertisement.Credit, result);
                UpdateReservationState(reservation, "Closed", signedInUserId);
            } else
            {
                ReserveToken(reservation.User, reservation.Advertisement.Credit, result);
                UpdateReservationState(reservation, "Ongoing", signedInUserId);
            }
            _dbContext.SaveChanges();

            result.OtherUser = _userConverter.ConvertToDto(reservation.User.Id == signedInUserId ? reservation.Advertisement.User : reservation.User);
            result.ReservationId = reservation.Id;
            result.AdvertisementId = reservation.Advertisement.Id;
            result.IsLender = reservation.User.Id == signedInUserId ? true : false;
            return result;
        }

        public bool IsReservationClosed(int reservationId)
        {
            return _dbContext.Reservations
                        .Where(r => r.Id == reservationId)
                        .Select(r => r.ReservationState == ReservationState.Closed)
                        .FirstOrDefault();
        }

        private void TransferCredit(ApplicationUser lender, ApplicationUser advertiser, int? credit, TransactionOperationDto result)
        {
            result.Operation = "Close";

            if (credit == null || credit != 0)
            {
                result.Succeeded = true;
            }
            else if (credit < lender.ReservedCredit)
            {
                lender.ReservedCredit = lender.ReservedCredit - (int)credit;
                advertiser.Credit = advertiser.Credit + (int)credit;

                result.Succeeded = true;
                result.Credit = (int)credit;
                result.Message = credit + " credit has been transfered.";
            }
            else if (credit < lender.ReservedCredit + lender.Credit)
            {
                var notReservedCredits = (int)credit - lender.ReservedCredit;
                lender.ReservedCredit = 0;
                lender.Credit = lender.Credit - notReservedCredits;

                result.Succeeded = true;
                result.Credit = (int)credit;
                result.Message = credit + " credit has been transfered.";
            } else
            {
                var allCredit = lender.ReservedCredit + lender.Credit;
                lender.ReservedCredit = 0;
                lender.Credit = 0;
                advertiser.Credit = allCredit;

                result.Succeeded = false;
                result.Credit = (int)credit;
                result.Message = "Lender does not have enough credit..., " + allCredit + " has been transfered.";
            }
            _dbContext.Update(lender);
            _dbContext.Update(advertiser);
        }

        private void ReserveToken(ApplicationUser lender, int? credit, TransactionOperationDto result)
        {
            result.Operation = "Reserve";

            if (credit > lender.Credit)
            {
                result.Succeeded = false;
                result.Message = "Lender does not have enough credit!";
            }

            if (credit != null && credit != 0)
            {
                lender.Credit = lender.Credit - (int)credit;
                lender.ReservedCredit = (int)credit;

                result.Succeeded = true;
                result.Credit = (int)credit;
            }
            result.Succeeded = true;
            _dbContext.Update(lender);
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
                case "Ongoing":
                    return ReservationState.Ongoing;
                case "Closed":
                    return ReservationState.Closed;
                default:
                    return ReservationState.Created;
            }
        }
    }
}
