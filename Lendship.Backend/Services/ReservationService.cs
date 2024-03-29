﻿using Lendship.Backend.DTO;
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
using ReservedCredit = Lendship.Backend.Models.ReservedCredit;

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
            _notificationService.CreateNotification("New reservation created", reservation, advertisement.User.Id);
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
                reservation.AdmittedByLender = true;
                _notificationService.CreateNotification("Reservation was evaluated by the Lender", reservation, reservation.Advertisement.User.Id);
            }

            if (signedInUserId == reservation.Advertisement.User.Id)
            {
                reservation.AdmittedByAdvertiser = true;
                _notificationService.CreateNotification("Reservation was evaluated by the Advertiser", reservation, reservation.User.Id);
            }

            if (reservation.AdmittedByAdvertiser && reservation.AdmittedByLender)
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

        private void UpdateReservationState(Reservation reservation, string state, string signedInUserId, string additionalMsg = "")
        {
            var reservationState = GetReservationState(state);



            if (reservationState == ReservationState.Resigned || reservationState == ReservationState.Declined)
            {
                _notificationService.CreateNotification("Reservation was deleted, because it was " + reservationState, reservation, signedInUserId == reservation.User.Id ? reservation.Advertisement.User.Id : reservation.User.Id);

                _reservationRepository.Delete(reservation);
            }
            else if (reservationState == ReservationState.Ongoing || reservationState == ReservationState.Closed)
            {
                _notificationService.CreateNotification("Reservation state changed: " + reservationState + ". " + additionalMsg, reservation, reservation.User.Id);
                _notificationService.CreateNotification("Reservation state changed: " + reservationState + ". " + additionalMsg, reservation, reservation.Advertisement.User.Id);

                reservation.ReservationState = reservationState;
                _reservationRepository.Update(reservation);
            }
            else
            {
                _notificationService.CreateNotification("Reservation state changed: " + reservationState, reservation, signedInUserId == reservation.User.Id ? reservation.Advertisement.User.Id : reservation.User.Id);

                reservation.ReservationState = reservationState;
                _reservationRepository.Update(reservation);
            }
        }

        public IEnumerable<ReservationDto> GetReservationsForAdvertisement(int advertisementId)
        {
            var reservations = _reservationRepository.GetByAdvertisement(advertisementId)
                .Select(x => _reservationConverter.ConvertToDto(x));

            return reservations;
        }

        public void RemoveUpcommingReservations(int advertisementId)
        {
            var reservations = _reservationRepository.GetByAdvertisement(advertisementId).ToList();
            foreach (var res in reservations)
            {
                _notificationService.CreateNotification("Advertisement was deleted", res, res.User.Id);
                _notificationService.CreateNotification("Reservation was deleted, because you deleted the advertisement", res, res.Advertisement.User.Id);
            }

            _reservationRepository.RemoveUpcommingReservations(advertisementId);
        }

        public void RemoveUpcommingReservationForAvailabilities(int advertisementId, IEnumerable<Availability> availabilities)
        {
            var reservations = new List<Reservation>();

            foreach (var a in availabilities)
            {
                var res = _reservationRepository.GetByAdvertisement(advertisementId)
                    .Where(x => 
                        (x.DateFrom.CompareTo(a.DateFrom) < 0 && x.DateTo.CompareTo(a.DateFrom) >= 0) ||
                        (x.DateFrom.CompareTo(a.DateFrom) >= 0 && x.DateFrom.CompareTo(a.DateTo) < 0) ||
                        (x.DateTo.CompareTo(a.DateFrom) >= 0 && x.DateTo.CompareTo(a.DateTo) < 0));
                reservations.AddRange(res);
            }

            foreach (var res in reservations.ToList())
            {
                _notificationService.CreateNotification("Availibility of the Advertisement was updated, reservation is deleted", res, res.User.Id);
                _notificationService.CreateNotification("Reservation was deleted, because you updated the availibility", res, res.Advertisement.User.Id);
            }

            _reservationRepository.Delete(reservations);
        }

        public IEnumerable<ReservationForAdvertisementDto> GetRecentReservations()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = _reservationRepository.GetRecentReservations(signedInUserId)
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
                OtherUser = _userConverter.ConvertToDto(reservation.User.Id == signedInUserId ? reservation.Advertisement.User : reservation.User),
                ReservationId = reservation.Id,
                AdvertisementId = reservation.Advertisement.Id,
                IsLender = reservation.User.Id == signedInUserId ? true : false,
                IsAdmitted = reservation.User.Id == signedInUserId ? reservation.AdmittedByLender : reservation.AdmittedByAdvertiser
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

            if (reservation == null || reservation.ReservationState == ReservationState.Closed || !(time >= DateTime.UtcNow.AddMinutes(-5).Ticks && time < DateTime.UtcNow.Ticks))
            {
                return result;
            }

            if (closing == 1)
            {
                TransferCredit(reservation, result);
                UpdateReservationState(reservation, "Closed", signedInUserId, result.Credit + " was transfered");
            } else
            {
                ReserveCredit(reservation, result);
                UpdateReservationState(reservation, "Ongoing", signedInUserId, result.Credit + " was reserved");
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

        private void TransferCredit(Reservation reservation, TransactionOperationDto result)
        {
            result.Operation = "Close";

            var reservedCredits = reservation.User.ReservedCredits.Where(x => x.ReservationId == reservation.Id).FirstOrDefault();
            var reservedCreditAmount = reservedCredits != null ? reservedCredits.Amount : 0;
            var lender = reservation.User;
            var advertiser = reservation.Advertisement.User;

            if (reservation.Advertisement.Credit == null || reservation.Advertisement.Credit == 0)
            {
                result.Succeeded = true;
            }
            else if (reservation.Advertisement.Credit == reservedCreditAmount)
            {
                advertiser.Credit = advertiser.Credit + reservedCreditAmount;

                result.Succeeded = true;
                result.Credit = (int)reservation.Advertisement.Credit;
                result.Message = reservation.Advertisement.Credit + " credit has been transfered.";
            }
            else if (reservation.Advertisement.Credit < reservedCreditAmount + lender.Credit)
            {
                var notReservedCredits = (int)reservation.Advertisement.Credit - reservedCreditAmount;

                lender.Credit = lender.Credit - notReservedCredits;
                advertiser.Credit = advertiser.Credit + notReservedCredits + reservedCreditAmount;

                result.Succeeded = true;
                result.Credit = (int)reservation.Advertisement.Credit;
                result.Message = reservation.Advertisement.Credit + " credit has been transfered.";
            } else
            {
                var allCredit = reservedCreditAmount + lender.Credit;

                lender.Credit = 0;
                advertiser.Credit = advertiser.Credit + allCredit;

                result.Succeeded = false;
                result.Credit = (int)reservation.Advertisement.Credit;
                result.Message = "Lender does not have enough credit..., " + allCredit + " has been transfered.";
            }

            RemoveReservedCredits(reservation, reservedCredits);

            _userRepository.Update(lender);
            _userRepository.Update(advertiser);
        }

        private void RemoveReservedCredits(Reservation reservation, ReservedCredit reservedCredits)
        {
            reservation.User.ReservedCredits.Remove(reservedCredits);
            _reservationRepository.Update(reservation);
        }

        private void SaveReservedCredits(Reservation reservation, int amount)
        {
            var reservedCredits = new ReservedCredit()
            {
                UserId = reservation.User.Id,
                ReservationId = reservation.Id,
                Amount = amount,
            };

            reservation.User.ReservedCredits.Add(reservedCredits);
            _reservationRepository.Update(reservation);
        }

        private void ReserveCredit(Reservation reservation, TransactionOperationDto result)
        {
            result.Operation = "Reserve";

            var credit = reservation.Advertisement.Credit;
            var lender = reservation.User;

            if (credit > lender.Credit)
            {
                result.Succeeded = false;
                result.Message = "Lender does not have enough credit!";
            }

            if (credit != null && credit != 0)
            {
                lender.Credit = lender.Credit - (int)credit;
                SaveReservedCredits(reservation, (int)credit);

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
