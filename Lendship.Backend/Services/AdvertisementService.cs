using GeoCoordinatePortable;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Lendship.Backend.Repositories;
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
        private readonly IConversationRepository _conversationRepository;

        private readonly ICategoryService _categoryService;
        private readonly IReservationService _reservationService;
        private readonly IImageService _imageService;
        private readonly IPrivateUserService _privateUserService;

        private readonly IAdvertisementConverter _adDetailsConverter;
        private readonly IAdvertisementConverter _adConverter;
        private readonly IAvailabilityConverter _availabilityConverter;

        private readonly int _advertisementsPerPage = 10;

        public AdvertisementService(
            IHttpContextAccessor httpContextAccessor,
            IAdvertisementRepository advertisementRepository, 
            IUserRepository userRepository,
            ISavedAdvertisementRepository savedAdvertisementRepository,
            IAvailabilityRepository availabilityRepository,
            IConversationRepository conversationRepository,
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
            _conversationRepository = conversationRepository;

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

            _conversationRepository.DeleteByAdvertisementId(advertisementId);
            _reservationService.RemoveUpcommingReservations(advertisementId);
            _advertisementRepository.Delete(advertisementId);
        }

        public AdvertisementListDto GetAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy, int page)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userRepository.GetById(signedInUserId);
            var userLocation = new GeoCoordinate((double)user.Latitude, (double)user.Longitude);

            var ads = _advertisementRepository.GetAll(signedInUserId);

            var advertisements = FilterAdvertisments(ads, advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy);

            if (sortBy == null)
            {
                advertisements = advertisements.OrderBy(x => userLocation.GetDistanceTo(new GeoCoordinate((double)x.Latitude, (double)x.Longitude)));
            }

            var result = Paging(advertisements, page)
                    .Select(x => _adConverter.ConvertToDto(x));

            return new AdvertisementListDto()
            {
                Pages = GetPages(advertisements.Count()),
                Advertisements = result
            };
        }

        private int GetPages(int count)
        {
            return count % _advertisementsPerPage == 0
                ? count / _advertisementsPerPage
                : (count / _advertisementsPerPage) + 1;
        }

        public AdvertisementListDto GetUsersAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy, int page)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ads = _advertisementRepository.GetAll(signedInUserId)
                        .Where(a => a.User.Id == signedInUserId)
                        .ToList();

            var advertisements = FilterAdvertisments(ads, advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy);

            var result = Paging(advertisements, page)
                    .Select(x => _adConverter.ConvertToDto(x));

            return new AdvertisementListDto()
            {
                Pages = GetPages(advertisements.Count()),
                Advertisements = result
            };
        }

        public AdvertisementListDto GetSavedAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy, int page)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var savedAds = _savedAdvertisementRepository.GetSavedAdvertisementIdsByUser(signedInUserId).ToList();
            var advertisements = _advertisementRepository.GetAll(signedInUserId)
                        .Where(a => savedAds.Contains(a.Id));

            advertisements = FilterAdvertisments(advertisements, advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy);

            var result = Paging(advertisements, page)
                    .Select(x => _adConverter.ConvertToDto(x));

            return new AdvertisementListDto()
            {
                Pages = GetPages(advertisements.Count()),
                Advertisements = result
            };
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

        private IEnumerable<Advertisement> Paging(IEnumerable<Advertisement> ads, int page)
        {
            if (page != null)
            {
                var result = ads.Skip(_advertisementsPerPage * page)
                                .Take(_advertisementsPerPage);

                return result;
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
