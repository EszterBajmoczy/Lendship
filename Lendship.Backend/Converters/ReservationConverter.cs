using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Lendship.Backend.DTO.ReservationDto;

namespace Lendship.Backend.Converters
{
    public class ReservationConverter : IReservationConverter
    {
        private readonly IAdvertisementConverter _adConverter;

        public ReservationConverter(IAdvertisementConverter adConverter)
        {
            _adConverter = adConverter;
        }

        public ReservationDto ConvertToDto(Reservation reservation)
        {
            var adDto = _adConverter.ConvertToDto(reservation.Advertisement);
            return new ReservationDto
            {
                Id = reservation.Id,
                Advertisement = adDto,
                ReservationState = GetReservationEnumState(reservation.ReservationState),
                Comment = reservation.Comment,
                AdmittedByAdvertiser = reservation.admittedByAdvertiser,
                AdmittedByLender = reservation.admittedByAdvertiser,
                DateFrom = reservation.DateFrom,
                DateTo = reservation.DateTo,
            };
        }

        public Reservation ConvertToEntity(ReservationDto reservationDto, ApplicationUser user, Advertisement advertisement)
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
