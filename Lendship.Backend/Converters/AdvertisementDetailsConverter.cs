using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Converters
{
    public class AdvertisementDetailsConverter : IAdvertisementDetailsConverter
    {
        private readonly IUserConverter _userConverter;
        private readonly IAvailabilityConverter _availabillityConverter;
        private readonly ICategoryConverter _categoryConverter;

        public AdvertisementDetailsConverter(IUserConverter userConverter, IAvailabilityConverter availabillityConverter, ICategoryConverter categoryConverter)
        {
            _userConverter = userConverter;
            _availabillityConverter = availabillityConverter;
            _categoryConverter = categoryConverter;
        }

        public AdvertisementDetailsDto ConvertToDto(Advertisement ad)
        {
            if(ad == null)
            {
                return null;
            }

            var userDTO = _userConverter.ConvertToDto(ad.User);
            var privateUserDTOs = new List<UserDto>();

            if (ad.PrivateUsers != null && ad.PrivateUsers.Count > 0)
            {
                privateUserDTOs = ad.PrivateUsers.Select(x => _userConverter.ConvertToDto(x.User)).ToList();
            }

            IEnumerable<AvailabilityDto> availabilityDtos = ad.Availabilities.Select(a => _availabillityConverter.ConvertToDto(a));

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
                Location = ad.Location,
                IsPublic = ad.IsPublic,
                Category = _categoryConverter.ConvertToDto(ad.Category),
                Availabilities = availabilityDtos.ToList(),
                ImageLocations = ad.ImageLocations.Select(i => i.Location).ToList(),
                PrivateUsers = privateUserDTOs,
                Creation = ad.Creation
            };
        }

        public Advertisement ConvertToEntity(AdvertisementDetailsDto ad, ApplicationUser user, Category category)
        {
            //var availabilities = ad.Availabilities.Select(a => _availabillityConverter.ConvertToEntity(a, ad));

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
                Location = ad.Location,
                IsPublic = ad.IsPublic ?? true,
                Category = category,
                Creation = ad.Creation ?? DateTime.Now
            };

            return newAd;
        }
    }
}
