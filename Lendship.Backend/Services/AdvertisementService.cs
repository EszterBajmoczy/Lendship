using GeoCoordinatePortable;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISavedAdvertisementRepository _savedAdvertisementRepository;
        private readonly IAvailabilityRepository _availabilityRepository;

        private readonly ICategoryService _categoryService;
        private readonly IReservationService _reservationService;
        private readonly IImageService _imageService;
        private readonly IPrivateUserService _privateUserService;

        private readonly IAdvertisementConverter _adDetailsConverter;
        private readonly IAdvertisementConverter _adConverter;
        private readonly IAvailabilityConverter _availabilityConverter;

        public AdvertisementService(
            IHttpContextAccessor httpContextAccessor,
            IAdvertisementRepository advertisementRepository, 
            
            IUserRepository userRepository,
            ISavedAdvertisementRepository savedAdvertisementRepository,
            IAvailabilityRepository availabilityRepository,
            ICategoryService categoryService,
            IReservationService reservationService, 
            IImageService imageService,
            IPrivateUserService privateUserService,
            IAdvertisementConverter advertisementDetailsConverter,
            IAdvertisementConverter advertisementConverter,
            IAvailabilityConverter availabilityConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _advertisementRepository = advertisementRepository;
            _userRepository = userRepository;
            _savedAdvertisementRepository = savedAdvertisementRepository;
            _availabilityRepository = availabilityRepository;

            _categoryService = categoryService;
            _reservationService = reservationService;
            _imageService = imageService;
            _privateUserService = privateUserService;

            _adDetailsConverter = advertisementDetailsConverter;
            _adConverter = advertisementConverter;
            _availabilityConverter = availabilityConverter;
        }

        public AdvertisementDetailsDto GetAdvertisement(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var advertisement = _advertisementRepository.GetById(advertisementId, signedInUserId);
            
            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            var advertisementDto = _adDetailsConverter.ConvertToDetailsDto(advertisement);
            return advertisementDto;
        }

        public int CreateAdvertisement(AdvertisementDetailsDto advertisement)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var category = _categoryService.GetOrCreateCategoryByName(advertisement.Category.Name);
            var user = _userRepository.GetById(signedInUserId);

            var ad =_adDetailsConverter.ConvertToEntity(advertisement, user, category);

            _advertisementRepository.Create(ad);

            UpdateAvailabilities(ad, advertisement.Availabilities);
            _privateUserService.UpdatePrivateUsers(ad.Id, advertisement.PrivateUsers);

            return ad.Id;
        }

        public void UpdateAdvertisement(AdvertisementDetailsDto advertisement)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var oldAd = _advertisementRepository.GetById(advertisement.Id, signedInUserId);

            if (oldAd == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            if (oldAd.User.Id != signedInUserId)
            {
                throw new UpdateNotAllowedException("Update not allowed.");
            }

            var category = _categoryService.GetOrCreateCategoryByName(advertisement.Category.Name);

            var user = _userRepository.GetById(signedInUserId);
            var privateUsers = advertisement.PrivateUsers.Select(x => _userRepository.GetById(x.Id.ToString()));

            var ad = _adDetailsConverter.ConvertToEntity(advertisement, user, category);

            _advertisementRepository.Update(ad);

            UpdateAvailabilities(ad, advertisement.Availabilities);
            _privateUserService.UpdatePrivateUsers(ad.Id, advertisement.PrivateUsers);
        }

        public void DeleteAdvertisement(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var advertisement = _advertisementRepository.GetById(advertisementId, signedInUserId);


            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found");
            }

            _imageService.DeleteImages(advertisementId);

            _advertisementRepository.Delete(advertisementId);
            _reservationService.RemoveUpcommingReservations(advertisementId);
        }

        public IEnumerable<AdvertisementDto> GetAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ads = _advertisementRepository.GetAll(signedInUserId);

            return FilterAdvertisments(ads, advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy)
                    .Select(x => _adConverter.ConvertToDto(x));
        }

        public IEnumerable<AdvertisementDto> GetUsersAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ads = _advertisementRepository.GetAll(signedInUserId)
                        .Where(a => a.User.Id == signedInUserId)
                        .ToList();

            return FilterAdvertisments(ads, advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy)
                .Select(x => _adConverter.ConvertToDto(x));
        }

        public IEnumerable<AdvertisementDto> GetSavedAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var savedAds = _savedAdvertisementRepository.GetSavedAdvertisementIdsByUser(signedInUserId).ToList();
            var advertisements = _advertisementRepository.GetAll(signedInUserId)
                        .Where(x => x.User.Id == signedInUserId || (!x.IsPublic && x.PrivateUsers.Any(p => p.UserId == signedInUserId)))
                        .Where(a => savedAds.Contains(a.Id))
                        .ToList();

            return FilterAdvertisments(advertisements, advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy)
                .Select(x => _adConverter.ConvertToDto(x));
        }

        public void RemoveSavedAdvertisement(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            _savedAdvertisementRepository.DeleteAll(signedInUserId, advertisementId);
        }

        public void SaveAdvertisementForUser(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var advertisement = _advertisementRepository.GetById(advertisementId, signedInUserId);

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found");
            }
            
            var savedAd = _savedAdvertisementRepository.Get(signedInUserId, advertisementId);
            if(savedAd == null)
            {
                var newSaveAd = new SavedAdvertisement
                {
                    UserId = signedInUserId,
                    AdvertisementId = advertisementId
                };
                _savedAdvertisementRepository.Create(newSaveAd);
            }
        }

        public bool IsAdvertisementSaved(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var connection = _savedAdvertisementRepository.Get(signedInUserId, advertisementId);

            return connection != null;
        }

        private IEnumerable<Advertisement> FilterAdvertisments(IEnumerable<Advertisement> ads, string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            if (advertisementType != null && advertisementType.Length > 0)
            {
                ads = ads.Where(a => a.AdvertisementType.ToString() == advertisementType);
            }

            if (creditPayment && cashPayment)
            {
                ads = ads.Where(a => a.Credit != null || a.Price != null);
            }
            else if (creditPayment)
            {
                ads = ads.Where(a => a.Credit != null);
            }
            else if (cashPayment)
            {
                ads = ads.Where(a => a.Price != null);
            }

            if (category != null && category != "")
            {
                ads = ads.Where(a => a.Category.Name.ToLower() == category.ToLower());
            }

            if (word != null)
            {
                ads = ads.Where(a => a.Title.Contains(word) || a.Description.Contains(word));
            }

            //TODO city and distance!!

            if (latitude > 0 && longitude > 0 && distance > 0)
            {
                var distanceInMeter = distance * 1000;
                ads = ads.Where(a => (new GeoCoordinate((double)a.Latitude, (double)a.Longitude)).GetDistanceTo(new GeoCoordinate(latitude, longitude)) <= distanceInMeter);
            }

            switch (sortBy)
            {
                case "Price":
                    ads = ads.OrderBy(a => a.Price);
                    break;
                case "Credit":
                    ads = ads.OrderBy(a => a.Credit);
                    break;
                case "Creation":
                    ads = ads.OrderBy(a => a.Creation);
                    break;
                default:
                    break;
            }

            return ads;
        }

        private void UpdateAvailabilities(Advertisement ad, List<AvailabilityDto> availabilities)
        {
            var savedAv = _availabilityRepository.GetByAdvertisement(ad.Id);

            var avIds = availabilities
                            .Where(a => a.Id != 0)
                            .Select(a => a.Id);

            var toAdd = availabilities.Where(a => a.Id == 0).Select(a => _availabilityConverter.ConvertToEntity(a, ad));
            var toDelete = savedAv.Where(a => !avIds.Contains(a.Id));

            _availabilityRepository.DeleteRange(toDelete);
            _availabilityRepository.AddRange(toAdd);
        }
    }
}
