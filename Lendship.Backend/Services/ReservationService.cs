using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static Lendship.Backend.DTO.ReservationDetailDto;

namespace Lendship.Backend.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IUserRepository _userRepository;

        private readonly IReservationConverter _reservationConverter;
        private readonly IUserConverter _userConverter;

        public ReservationService( 
            IHttpContextAccessor httpContextAccessor,
            INotificationService notificationService,
            IReservationRepository reservationRepository,
            IAdvertisementRepository advertisementRepository,
            IUserRepository userRepository,
            IReservationConverter reservationConverter,
            IUserConverter userConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
            _reservationRepository = reservationRepository;
            _advertisementRepository = advertisementRepository;
            _userRepository = userRepository;

            _reservationConverter = reservationConverter;
            _userConverter = userConverter;
        }
                
        public void CreateReservation(ReservationDetailDto reservationDto, int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _userRepository.GetById(signedInUserId);
            var advertisement = _advertisementRepository.GetPlainById(advertisementId, signedInUserId);

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not exists.");
            }

            if (signedInUserId == advertisement.User.Id)
            {
                throw new Exception("Can not make reservation for own advertisement.");
            }

            var reservation = _reservationConverter.ConvertToEntity(reservationDto, user, advertisement);

            _reservationRepository.Create(reservation);
        }

        public IEnumerable<ReservationDetailDto> GetReservations()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _reservationRepository.GetByUser(signedInUserId)
                .Select(x => _reservationConverter.ConvertToDetailDto(x))
                .OrderByDescending(x => x.DateFrom);

            return reservations;
        }

        public IEnumerable<ReservationDetailDto> GetReservationsForUser()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservations = _reservationRepository.GetForUserAdvertisement(signedInUserId)
                                .Select(r => _reservationConverter.ConvertToDetailDto(r))
                                .OrderByDescending(x => x.DateFrom);

            return reservations;
        }

        public void UpdateReservation(ReservationDetailDto reservationDto)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var oldRes = _reservationRepository.GetById(reservationDto.Id);

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

            if (reservationDto.ReservationState == ReservationStateEnum.ClosedEnum)
            {
                throw new UpdateNotAllowedException("Not correct method to close the conversation.");
            }

            var reservation = _reservationConverter.ConvertToEntity(reservationDto, oldRes.User, oldRes.Advertisement);
            _reservationRepository.Update(reservation);
        }

        public void AdmitReservation(int reservationId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = _reservationRepository.GetById(reservationId);

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
            _reservationRepository.Update(reservation);
        }

        public void UpdateReservationState(int reservationId, string state)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = _reservationRepository.GetById(reservationId);

            if (reservation == null)
            {
                throw new ReservationNotFoundException("Reservation not found.");
            }

            if (reservation.Advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            if (GetReservationState(state) == ReservationState.Closed)
            {
                throw new UpdateNotAllowedException("Not correct method to close the conversation.");
            }

            if (signedInUserId != reservation.User.Id && signedInUserId != reservation.Advertisement.User.Id)
            {
                throw new UpdateNotAllowedException("Update not allowed.");
            }

            UpdateReservationState(reservation, state, signedInUserId);
        }

        private void UpdateReservationState(Reservation reservation, string state, string signedInUserId)
        {
            var reservationState = GetReservationState(state);

            _notificationService.CreateNotification("Reservation state changed: " + reservationState, reservation, signedInUserId == reservation.User.Id ? signedInUserId : reservation.Advertisement.User.Id);

            reservation.ReservationState = reservationState;

            _reservationRepository.Update(reservation);
        }

        public IEnumerable<ReservationDto> GetReservationsForAdvertisement(int advertisementId)
        {
            var reservations = _reservationRepository.GetByAdvertisement(advertisementId)
                .Select(x => _reservationConverter.ConvertToDto(x));

            return reservations;
        }

        public void RemoveUpcommingReservations(int advertisementId)
        {
            var reservations = _reservationRepository.GetByAdvertisement(advertisementId);
            foreach (var res in reservations)
            {
                _notificationService.CreateNotification("Advertisement was deleted", res, res.User.Id);
                _notificationService.CreateNotification("Reservation was deleted, because you deleted the advertisement", res, res.Advertisement.User.Id);
            }

            _reservationRepository.RemoveUpcommingReservations(advertisementId);
        }

        public IEnumerable<ReservationForAdvertisementDto> GetRecentReservations()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = _reservationRepository.GetRecentReservations(signedInUserId)
                                .Where(x => x.ReservationState != ReservationState.Closed)
                                .OrderByDescending(x => x.DateFrom)
                                .Select(r => _reservationConverter.ConvertToReservationForAdvertisementDto(r, true))
                                .ToList();


            return reservations;
        }

        public ReservationTokenDto GetReservationToken(int reservationId, bool closing)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = _reservationRepository.GetById(reservationId);

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

            return new ReservationTokenDto()
            {
                ReservationToken = builder.ToString(),
                OtherUser = _userConverter.ConvertToDto(reservation.User.Id == signedInUserId ? reservation.User : reservation.Advertisement.User),
                ReservationId = reservation.Id,
                AdvertisementId = reservation.Advertisement.Id,
                IsLender = reservation.User.Id == signedInUserId ? true : false
            };
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

            var reservation = _reservationRepository.GetReservation(userId, signedInUserId, reservationId);

            if (reservation == null || !(time >= DateTime.UtcNow.AddMinutes(-5).Ticks && time < DateTime.UtcNow.Ticks))
            {
                return result;
            }

            if (closing == 1)
            {
                TransferCredit(reservation.User, reservation.Advertisement.User, reservation.Advertisement.Credit, result);
                UpdateReservationState(reservation, "Closed", signedInUserId);
            } else
            {
                ReserveCredit(reservation.User, reservation.Advertisement.Credit, result);
                UpdateReservationState(reservation, "Ongoing", signedInUserId);
            }

            result.OtherUser = _userConverter.ConvertToDto(reservation.User.Id == signedInUserId ? reservation.Advertisement.User : reservation.User);
            result.ReservationId = reservation.Id;
            result.AdvertisementId = reservation.Advertisement.Id;
            result.IsLender = reservation.User.Id == signedInUserId ? true : false;
            return result;
        }

        public bool IsReservationClosed(int reservationId)
        {
            return _reservationRepository.IsReservationClosed(reservationId);
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

            _userRepository.Update(lender);
            _userRepository.Update(advertiser);
        }

        private void ReserveCredit(ApplicationUser lender, int? credit, TransactionOperationDto result)
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
                result.Message = credit + " credit was reserved.";
            }
            result.Succeeded = true;
            _userRepository.Update(lender);
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
