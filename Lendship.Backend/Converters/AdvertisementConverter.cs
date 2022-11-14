using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using static Lendship.Backend.DTO.AdvertisementDetailsDto;

namespace Lendship.Backend.Converters
{
    public class AdvertisementConverter : IAdvertisementConverter
    {
        private readonly IUserConverter _userConverter;
        private readonly IAvailabilityConverter _availabillityConverter;
        private readonly ICategoryConverter _categoryConverter;

        public AdvertisementConverter(IUserConverter userConverter, IAvailabilityConverter availabillityConverter, ICategoryConverter categoryConverter)
        {
            _userConverter = userConverter;
            _availabillityConverter = availabillityConverter;
            _categoryConverter = categoryConverter;
        }

        public AdvertisementDto ConvertToDto(Advertisement ad)
        {
            return new AdvertisementDto
            {
                Id = ad.Id,
                User = ad.User != null ? _userConverter.ConvertToDto(ad.User) : null,
                Title = ad.Title,
                Price = ad.Price,
                Credit = ad.Credit,
                Latitude = ad.Latitude,
                Longitude = ad.Longitude,
                Location = ad.Location,
                ImageLocation = ad.ImageLocations?.FirstOrDefault()?.Location
            };
        }

        public AdvertisementDetailsDto ConvertToDetailsDto(Advertisement ad)
        {
            if (ad == null)
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
                AdvertisementType = GetAdvertisementTypeEnumState(ad.AdvertisementType),
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
                AdvertisementType = GetAdvertisementTypeState(ad.AdvertisementType),
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

        private AdvertisementTypeEnum GetAdvertisementTypeEnumState(AdvertisementType state)
        {
            return state switch
            {
                AdvertisementType.Property => AdvertisementTypeEnum.PropertyEnum,
                AdvertisementType.Service => AdvertisementTypeEnum.ServiceEnum,
                _ => AdvertisementTypeEnum.PropertyEnum,
            };
        }

        private AdvertisementType GetAdvertisementTypeState(AdvertisementTypeEnum? state)
        {
            return state switch
            {
                AdvertisementTypeEnum.PropertyEnum => AdvertisementType.Property,
                AdvertisementTypeEnum.ServiceEnum => AdvertisementType.Service,
                _ => AdvertisementType.Property,
            };
        }
    }
}
