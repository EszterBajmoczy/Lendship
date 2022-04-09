using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Converters
{
    public class AdvertisementDetailsConverter : IAdvertisementDetailsConverter
    {
        private readonly IUserConverter _userConverter;
        private readonly IAvailabilityConverter _availabillityConverter;

        public AdvertisementDetailsConverter(IUserConverter userConverter, IAvailabilityConverter availabillityConverter)
        {
            _userConverter = userConverter;
            _availabillityConverter = availabillityConverter;
        }

        public AdvertisementDetailsDto ConvertToDto(Advertisement ad, IEnumerable<Availability> availabilities)
        {
            if(ad == null)
            {
                return null;
            }

            //TODO evaluations !!!
            var userDTO = _userConverter.ConvertToDto(ad.User);

            List<AvailabilityDto> availabilityDtos = new List<AvailabilityDto>();

            foreach (var availability in availabilities)
            {
                var a = _availabillityConverter.ConvertToDto(availability);
                availabilityDtos.Add(a);
            }

            return new AdvertisementDetailsDto
            {
                Id = ad.Id,
                User = userDTO,
                Title = ad.Title,
                Description = ad.Description,
                InstructionManual = ad.InstructionManual,
                Price = ad.Price,
                Credit = ad.Credit,
                Deposit = ad.Deposit,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                IsPublic = ad.IsPublic,
                Category = ad.Category.Name,
                Availabilities = availabilityDtos,
                ImageLocations = ad.ImageLocations.Select(i => i.Location).ToList(),
                Creation = ad.Creation
            };
        }

        public Advertisement ConvertToEntity(AdvertisementDetailsDto ad, ApplicationUser user, Category category)
        {
            var newAd = new Advertisement
            {
                Id = ad.Id ?? 0,
                User = user,
                Title = ad.Title,
                Description = ad.Description,
                InstructionManual = ad.InstructionManual,
                Price = ad.Price,
                Credit = ad.Credit,
                Deposit = ad.Deposit,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                IsPublic = ad.IsPublic ?? true,
                Category = category,
                Creation = ad.Creation ?? DateTime.Now
            };

            return newAd;
        }
    }
}
