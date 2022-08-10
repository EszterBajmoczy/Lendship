﻿using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
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
        private readonly LendshipDbContext _dbContext;
        private readonly AdvertisementDetailsConverter _adDetailsConverter;
        private readonly AdvertisementConverter _adConverter;

        public AdvertisementService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            _adDetailsConverter = new AdvertisementDetailsConverter(new UserConverter(), new AvailabilityConverter());
            _adConverter = new AdvertisementConverter();
        }

        public AdvertisementDetailsDto GetAdvertisement(int advertisementId)
        {
            var advertisement = _dbContext.Advertisements
                .Include(a => a.User)
                .Include(a => a.Category)
                .Include(a => a.ImageLocations)
                .Include(a => a.Availabilities)
                .Where(a => a.Id == advertisementId)
                .FirstOrDefault();

            var advertisementDto = _adDetailsConverter.ConvertToDto(advertisement);
            return advertisementDto;
        }

        public int CreateAdvertisement(AdvertisementDetailsDto advertisement)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var category = _dbContext.Categories.Where(x => x.Name == advertisement.Category).FirstOrDefault();
            var user = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();

            if(category == null)
            {
                throw new CategoryNotExistsException("Category not exists.");
            }

            var ad =_adDetailsConverter.ConvertToEntity(advertisement, user, category);

            _dbContext.Advertisements.Add(ad);
            _dbContext.SaveChanges();

            return ad.Id;
        }

        public void UpdateAdvertisement(AdvertisementDetailsDto advertisement)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (new Guid(signedInUserId) != advertisement.User.Id)
            {
                throw new UpdateNotAllowedException("Update is not allowed.");
            }
            var oldAd = _dbContext.Advertisements
                            .AsNoTracking()
                            .Include(a => a.User)
                            .Where(a => a.Id == advertisement.Id)
                            .FirstOrDefault();

            if(oldAd == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            if (oldAd.User.Id != signedInUserId)
            {
                throw new UpdateNotAllowedException("Update not allowed.");
            }

            var category = _dbContext.Categories.Where(x => x.Name == advertisement.Category).FirstOrDefault();

            if (category == null)
            {
                throw new CategoryNotExistsException("Category not exists.");
            }

            var user = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();

            var ad = _adDetailsConverter.ConvertToEntity(advertisement, user, category);

            _dbContext.Update(ad);
            _dbContext.SaveChanges();
        }

        public void DeleteAdvertisement(int advertisementId)
        {
            var advertisement = _dbContext.Advertisements
                .Where(a => a.Id == advertisementId)
                .FirstOrDefault();

            if(advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found");
            }

            _dbContext.Advertisements.Remove(advertisement);
            _dbContext.SaveChanges();
        }

        public IEnumerable<AdvertisementDto> GetAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy)
        {
            //TODO query string
            var resultList = new List<AdvertisementDto>();

            var ads = _dbContext.Advertisements
                        .Include(a => a.ImageLocations)
                        .ToList();

            ads = FilterAdvertisments(ads, advertisementType, creditPayment, cashPayment, category, city, distance, word, sortBy);

            foreach (var ad in ads)
            {
                var dto = _adConverter.ConvertToDto(ad);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<AdvertisementDto> GetUsersAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy)
        {
            //TODO query string
            var resultList = new List<AdvertisementDto>();
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ads = _dbContext.Advertisements
                        .Include(a => a.ImageLocations)
                        .Where(a => a.User.Id == signedInUserId)
                        .ToList();

            ads = FilterAdvertisments(ads, advertisementType, creditPayment, cashPayment, category, city, distance, word, sortBy);

            foreach (var ad in ads)
            {
                var dto = _adConverter.ConvertToDto(ad);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<AdvertisementDto> GetSavedAdvertisements(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy)
        {
            //TODO query string
            var resultList = new List<AdvertisementDto>();

            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var savedAds = _dbContext.SavedAdvertisements.Where(sa => sa.UserId == signedInUserId).Select(a => a.AdvertisementId).ToList();
            var advertisements = _dbContext.Advertisements
                .Include(a => a.ImageLocations)
                .Where(a => savedAds.Contains(a.Id))
                .ToList();

            advertisements = FilterAdvertisments(advertisements, advertisementType, creditPayment, cashPayment, category, city, distance, word, sortBy);

            foreach(var ad in advertisements)
            {
                var dto = _adConverter.ConvertToDto(ad);
                resultList.Add(dto);
            }

            return resultList;
        }

        public void RemoveSavedAdvertisement(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var savedAds = _dbContext.SavedAdvertisements.Where(sa => sa.UserId == signedInUserId && sa.AdvertisementId == advertisementId).ToList();
            _dbContext.SavedAdvertisements.RemoveRange(savedAds);
            _dbContext.SaveChanges();
        }

        public void SaveAdvertisementForUser(int advertisementId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var advertisement = _dbContext.Advertisements
                .Where(a => a.Id == advertisementId)
                .FirstOrDefault();

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found");
            }
            
            var savedAd = _dbContext.SavedAdvertisements.Where(sa => sa.UserId == signedInUserId && sa.AdvertisementId == advertisementId).FirstOrDefault();
            if(savedAd == null)
            {
                var newSaveAd = new SavedAdvertisement
                {
                    UserId = signedInUserId,
                    AdvertisementId = advertisementId
                };
                _dbContext.SavedAdvertisements.Add(newSaveAd);
                _dbContext.SaveChanges();
            }
        }

        private List<Advertisement> FilterAdvertisments(List<Advertisement> ads, string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy)
        {
            var result = ads;
            //TODO advertisemnetType!!

            if (creditPayment && cashPayment)
            {
                ads = ads.Where(a => a.Credit != null || a.Price != null).ToList();
            }
            else if (creditPayment)
            {
                ads = ads.Where(a => a.Credit != null).ToList();
            }
            else if (cashPayment)
            {
                ads = ads.Where(a => a.Price != null).ToList();
            }

            if (category != null && category != "")
            {
                ads = ads.Where(a => a.Category.Name == category).ToList();
            }

            if (word != null)
            {
                ads = ads.Where(a => a.Title.Contains(word) || a.Description.Contains(word)).ToList();
            }

            //TODO city and distance!!

            switch (sortBy)
            {
                case "Price":
                    ads = ads.OrderBy(a => a.Price).ToList();
                    break;
                case "Credit":
                    ads = ads.OrderBy(a => a.Credit).ToList();
                    break;
                case "Creation":
                    ads = ads.OrderBy(a => a.Creation).ToList();
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
