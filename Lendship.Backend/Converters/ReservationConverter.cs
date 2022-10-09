using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using static Lendship.Backend.DTO.ReservationDetailDto;

namespace Lendship.Backend.Converters
{
    public class ReservationConverter : IReservationConverter
    {
        private readonly IAdvertisementConverter _adConverter;
        private readonly IUserConverter _userConverter;

        public ReservationConverter(IAdvertisementConverter adConverter, IUserConverter userConverter)
        {
            _adConverter = adConverter;
            _userConverter = userConverter;
        }

        public ReservationDetailDto ConvertToDetailDto(Reservation reservation)
        {
            var adDto = _adConverter.ConvertToDto(reservation.Advertisement);
            return new ReservationDetailDto
            {
                Id = reservation.Id,
                Advertisement = adDto,
                ReservationState = GetReservationEnumState(reservation.ReservationState),
                Comment = reservation.Comment,
                AdmittedByAdvertiser = reservation.admittedByAdvertiser,
                AdmittedByLender = reservation.admittedByAdvertiser,
                User = reservation.User != null ? _userConverter.ConvertToDto(reservation.User) : null,
                DateFrom = reservation.DateFrom,
                DateTo = reservation.DateTo,
            };
        }

        public ReservationDto ConvertToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                ReservationState = GetReservationEnumState(reservation.ReservationState),
                DateFrom = reservation.DateFrom,
                DateTo = reservation.DateTo,
            };
        }

        public ReservationForAdvertisementDto ConvertToReservationForAdvertisementDto(Reservation reservation)
        {
            return new ReservationForAdvertisementDto
            {
                ReservationId = reservation.Id,
                AdvertisementName = reservation.Advertisement.Title
            };
        }

        public Reservation ConvertToEntity(ReservationDetailDto reservationDto, ApplicationUser user, Advertisement advertisement)
        {
            var reservation = new Reservation
            {
                Id = reservationDto.Id ?? 0,
                User = user,
                Advertisement = advertisement,
                ReservationState = GetReservationState(reservationDto.ReservationState),
                Comment = reservationDto.Comment,
                admittedByAdvertiser = reservationDto.AdmittedByAdvertiser ?? false,
                admittedByLender = reservationDto.AdmittedByLender ?? false,
                DateFrom = reservationDto.DateFrom ?? DateTime.Now,
                DateTo = reservationDto.DateTo ?? DateTime.Now
            };

            return reservation;
        }

        private ReservationStateEnum GetReservationEnumState(ReservationState state)
        {
            return state switch
            {
                ReservationState.Created => ReservationStateEnum.CreatedEnum,
                ReservationState.Accepted => ReservationStateEnum.AcceptedEnum,
                ReservationState.Declined => ReservationStateEnum.DeclinedEnum,
                ReservationState.Resigned => ReservationStateEnum.ResignedEnum,
                ReservationState.Closed => ReservationStateEnum.ClosedEnum,
                _ => ReservationStateEnum.CreatedEnum,
            };
        }

        private ReservationState GetReservationState(ReservationStateEnum? state)
        {
            return state switch
            {
                ReservationStateEnum.CreatedEnum => ReservationState.Created,
                ReservationStateEnum.AcceptedEnum => ReservationState.Accepted,
                ReservationStateEnum.DeclinedEnum => ReservationState.Declined,
                ReservationStateEnum.ResignedEnum => ReservationState.Resigned,
                ReservationStateEnum.ClosedEnum => ReservationState.Closed,
                _ => ReservationState.Created,
            };
        }
    }
}
