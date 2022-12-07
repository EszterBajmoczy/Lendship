using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Lendship.Backend.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Tests.UnitTest.Services
{
    [TestFixture]
    internal class AdvertisementServiceTests
    {
        private IAdvertisementService _sut;

        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<IAdvertisementRepository> _advertisementRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<ISavedAdvertisementRepository> _savedAdvertisementRepository;
        private Mock<IAvailabilityRepository> _availabilityRepository;
        private Mock<IConversationRepository> _conversationRepository;

        private Mock<ICategoryService> _categoryService;
        private Mock<IReservationService> _reservationService;
        private Mock<IImageService> _imageService;
        private Mock<IPrivateUserService> _privateUserService;

        private Mock<IAdvertisementConverter> _adConverter;
        private Mock<IAvailabilityConverter> _availabilityConverter;

        private string userId = "UserId";

        [SetUp]
        public void Setup()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _advertisementRepository = new Mock<IAdvertisementRepository>();
            _userRepository = new Mock<IUserRepository>();
            _savedAdvertisementRepository = new Mock<ISavedAdvertisementRepository>();
            _availabilityRepository = new Mock<IAvailabilityRepository>();
            _conversationRepository = new Mock<IConversationRepository>();

            _categoryService = new Mock<ICategoryService>();
            _reservationService = new Mock<IReservationService>();
            _imageService = new Mock<IImageService>();
            _privateUserService = new Mock<IPrivateUserService>();

            _adConverter = new Mock<IAdvertisementConverter>();
            _availabilityConverter = new Mock<IAvailabilityConverter>();

            _sut = new AdvertisementService(
                _httpContextAccessor.Object,
                _advertisementRepository.Object,
                _userRepository.Object,
                _savedAdvertisementRepository.Object,
                _availabilityRepository.Object,
                _conversationRepository.Object,
                _categoryService.Object,
                _reservationService.Object,
                _imageService.Object,
                _privateUserService.Object,
                _adConverter.Object,
                _availabilityConverter.Object);

            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>())).Returns(new Claim("name", userId));
        }

        [TestCase]
        public void GetAdvertisement_Should_Throw_Exception_When_Advertisement_Not_Found()
        {
            try
            {
                var result = _sut.GetAdvertisement(1);
            } catch (AdvertisementNotFoundException e)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [TestCase]
        public void GetAdvertisement_Should_Return_Converted_Advertisement()
        {
            var ad = new Advertisement();
            var adDto = new AdvertisementDetailsDto();

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);
            _adConverter.Setup(x => x.ConvertToDetailsDto(ad)).Returns(adDto);

            var result = _sut.GetAdvertisement(1);

            Assert.That(result, Is.EqualTo(adDto));
        }

        [TestCase]
        public void CreateAdvertisement_Should_Call_GetOrCreateCategoryByName()
        {
            var category = new Category() { Name = "TestCategory" };

            var adDto = new AdvertisementDetailsDto()
            {
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, It.IsAny<ApplicationUser>(), category)).Returns(ad);

            var result = _sut.CreateAdvertisement(adDto);

            _categoryService.Verify(x => x.GetOrCreateCategoryByName(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void CreateAdvertisement_Should_Call_UserRepository()
        {
            var category = new Category() { Name = "TestCategory" };

            var adDto = new AdvertisementDetailsDto()
            {
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, It.IsAny<ApplicationUser>(), category)).Returns(ad);

            var result = _sut.CreateAdvertisement(adDto);

            _userRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void CreateAdvertisement_Should_Convert_Dto_And_Call_Create()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();

            var adDto = new AdvertisementDetailsDto()
            {
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            var result = _sut.CreateAdvertisement(adDto);

            _adConverter.Verify(x => x.ConvertToEntity(adDto, user, category), Times.Once);
            _advertisementRepository.Verify(x => x.Create(ad), Times.Once);
        }

        [TestCase]
        public void CreateAdvertisement_Should_Return_Id()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();

            var adDto = new AdvertisementDetailsDto()
            {
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            var result = _sut.CreateAdvertisement(adDto);

            Assert.That(result, Is.EqualTo(ad.Id));
        }

        [TestCase]
        public void CreateAdvertisement_Should_Add_Availabilities()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>()
                {
                    new AvailabilityDto() { Id = 0, DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) },
                    new AvailabilityDto() { Id = 0, DateFrom = DateTime.Now.AddDays(2), DateTo = DateTime.Now.AddDays(4) }
                };
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities
            };

            var ad = new Advertisement() { Id = 1 };

            IEnumerable<Availability> addedList = null;

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
            _availabilityConverter.Setup(x => x.ConvertToEntity(It.IsAny<AvailabilityDto>(), ad)).Returns(availabilityEntity);
            _availabilityRepository.Setup(x => x.AddRange(It.IsAny<IEnumerable<Availability>>()))
                .Callback<IEnumerable<Availability>>(list =>
                {
                    addedList = list;
                });

            var result = _sut.CreateAdvertisement(adDto);

            CollectionAssert.Contains(addedList, availabilityEntity);
            Assert.That(addedList.Count(), Is.EqualTo(2));
        }

        [TestCase]
        public void CreateAdvertisement_Should_Delete_Availabilities()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>();
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities
            };

            var ad = new Advertisement() { Id = 1 };

            IEnumerable<Availability> deletedList = null;

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
            _availabilityConverter.Setup(x => x.ConvertToEntity(It.IsAny<AvailabilityDto>(), ad)).Returns(availabilityEntity);
            _availabilityRepository.Setup(x => x.GetByAdvertisement(It.IsAny<int>())).Returns(new List<Availability>() { availabilityEntity });

            _availabilityRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Availability>>()))
                .Callback<IEnumerable<Availability>>(list =>
                {
                    deletedList = list;
                });

            var result = _sut.CreateAdvertisement(adDto);

            CollectionAssert.Contains(deletedList, availabilityEntity);
            Assert.That(deletedList.Count(), Is.EqualTo(1));
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Throw_Advertisement_Not_Found_Exception()
        {
            try
            {
                var adDto = new AdvertisementDetailsDto()
                {
                    Id = 1
                };

                _sut.UpdateAdvertisement(adDto);
            } catch (AdvertisementNotFoundException e)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Throw_Update_Not_Allowed_Exception()
        {
            try
            {
                var adDto = new AdvertisementDetailsDto() { Id = 1 };
                var ad = new Advertisement() { User = new ApplicationUser() { Id = "NotGoodUserId"} };

                _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);

                _sut.UpdateAdvertisement(adDto);
            }
            catch (UpdateNotAllowedException e)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Call_CategoryService()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>();
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Id = 1,
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities,
                PrivateUsers = new List<UserDto>()
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            _sut.UpdateAdvertisement(adDto);

            _categoryService.Verify(x => x.GetOrCreateCategoryByName(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Call_UserRepository()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>();
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Id = 1,
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities,
                PrivateUsers = new List<UserDto>()
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            _sut.UpdateAdvertisement(adDto);

            _userRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Call_PrivateUserService()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>();
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Id = 1,
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities,
                PrivateUsers = new List<UserDto>()
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            _sut.UpdateAdvertisement(adDto);

            _privateUserService.Verify(x => x.UpdatePrivateUsers(It.IsAny<int>(), It.IsAny<List<UserDto>>()), Times.Once);
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Call_Converter_And_Repository_Update()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>();
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Id = 1,
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities,
                PrivateUsers = new List<UserDto>()
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            _sut.UpdateAdvertisement(adDto);

            _adConverter.Verify(x => x.ConvertToEntity(adDto, user, category), Times.Once);
            _advertisementRepository.Verify(x => x.Update(ad), Times.Once);
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Save_New_Availabilities()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>()
                {
                    new AvailabilityDto() { Id = 0, DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) },
                    new AvailabilityDto() { Id = 0, DateFrom = DateTime.Now.AddDays(2), DateTo = DateTime.Now.AddDays(4) }
                };
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Id = 1,
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities,
                PrivateUsers = new List<UserDto>()
            };

            var ad = new Advertisement() 
            { 
                Id = 1,
                User = new ApplicationUser() { Id = userId } 
            };

            IEnumerable<Availability> addedList = null;

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);
            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
            _availabilityConverter.Setup(x => x.ConvertToEntity(It.IsAny<AvailabilityDto>(), ad)).Returns(availabilityEntity);
            _availabilityRepository.Setup(x => x.AddRange(It.IsAny<IEnumerable<Availability>>()))
                .Callback<IEnumerable<Availability>>(list =>
                {
                    addedList = list;
                });

            _sut.UpdateAdvertisement(adDto);

            CollectionAssert.Contains(addedList, availabilityEntity);
            Assert.That(addedList.Count(), Is.EqualTo(2));
        }

        [TestCase]
        public void UpdateAdvertisement_Should_Delete_Availabilities()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();
            var availabilities = new List<AvailabilityDto>();
            var availabilityEntity = new Availability() { DateFrom = DateTime.Now, DateTo = DateTime.Now.AddDays(1) };

            var adDto = new AdvertisementDetailsDto()
            {
                Id = 1,
                Category = new CategoryDto() { Name = "TestCategory" },
                Availabilities = availabilities,
                PrivateUsers = new List<UserDto>()
            };

            var ad = new Advertisement()
            {
                Id = 1,
                User = new ApplicationUser() { Id = userId }
            };

            IEnumerable<Availability> deletedList = null;

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(ad);
            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
            _availabilityConverter.Setup(x => x.ConvertToEntity(It.IsAny<AvailabilityDto>(), ad)).Returns(availabilityEntity);
            _availabilityRepository.Setup(x => x.GetByAdvertisement(It.IsAny<int>())).Returns(new List<Availability>() { availabilityEntity });

            _availabilityRepository.Setup(x => x.DeleteRange(It.IsAny<IEnumerable<Availability>>()))
                .Callback<IEnumerable<Availability>>(list =>
                {
                    deletedList = list;
                });

            _sut.UpdateAdvertisement(adDto);

            CollectionAssert.Contains(deletedList, availabilityEntity);
            Assert.That(deletedList.Count(), Is.EqualTo(1));
        }

        [TestCase]
        public void DeleteAdvertisement_Should_Throw_Advertisement_Not_Found()
        {
            try
            {
                _sut.DeleteAdvertisement(1);
            } catch (AdvertisementNotFoundException e)
            {
                Assert.Pass();
            }

            Assert.Fail();
        }

        [TestCase(1)]
        public void DeleteAdvertisement_Should_Call_ImageService(int advertisementId)
        {
            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(new Advertisement());

            _sut.DeleteAdvertisement(advertisementId);

            _imageService.Verify(x => x.DeleteImages(It.IsAny<int>()), Times.Once);
        }

        [TestCase(1)]
        public void DeleteAdvertisement_Should_Delete_Advertisement_And_Call_ReservationService(int advertisementId)
        {
            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<string>())).Returns(new Advertisement());

            _sut.DeleteAdvertisement(advertisementId);

            _advertisementRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
            _reservationService.Verify(x => x.RemoveUpcommingReservations(It.IsAny<int>()), Times.Once);
        }

        [TestCase("Service", false, false, "", 0, 0, 0, "", "")]
        [TestCase("Property", false, false, "", 0, 0, 0, "", "")]
        public void Filter_AdvertisementType(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var propertyAd = new Advertisement() { AdvertisementType = AdvertisementType.Property };
            var serviceAd = new Advertisement() { AdvertisementType = AdvertisementType.Service };
            var ads = new List<Advertisement>() { propertyAd, serviceAd };

            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5});

            if (advertisementType == "Property")
            {
                var propAdDto = new AdvertisementDto();
                _adConverter.Setup(x => x.ConvertToDto(propertyAd)).Returns(propAdDto);

                var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

                CollectionAssert.Contains(result.Advertisements, propAdDto);
                Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
            } else
            {
                var serviceAdDto = new AdvertisementDto();
                _adConverter.Setup(x => x.ConvertToDto(serviceAd)).Returns(serviceAdDto);

                var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

                CollectionAssert.Contains(result.Advertisements, serviceAdDto);
                Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
            }
        }

        [TestCase(null, true, false, "", 0, 0, 0, "", "")]
        public void Filter_CreditPayment(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var creditAd = new Advertisement() { Credit = 10 };
            var noCreditAd = new Advertisement() { Credit = 0 };
            var ads = new List<Advertisement>() { creditAd, noCreditAd };

            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5 });

            var creditAdDto = new AdvertisementDto();
            _adConverter.Setup(x => x.ConvertToDto(creditAd)).Returns(creditAdDto);

            var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

            CollectionAssert.Contains(result.Advertisements, creditAdDto);
            Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
        }


        [TestCase(null, false, true, "", 0, 0, 0, "", "")]
        public void Filter_CashPayment(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var cachAd = new Advertisement() { Price = 10 };
            var noCashAd = new Advertisement() { Price = 0 };
            var ads = new List<Advertisement>() { cachAd, noCashAd };

            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5 });

            var cashAdDto = new AdvertisementDto();
            _adConverter.Setup(x => x.ConvertToDto(cachAd)).Returns(cashAdDto);

            var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

            CollectionAssert.Contains(result.Advertisements, cashAdDto);
            Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
        }

        [TestCase(null, false, false, "Category", 0, 0, 0, "", "")]
        public void Filter_Category(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var ad1 = new Advertisement() { Category = new Category() { Name = "Category" } };
            var ad2 = new Advertisement() { Category = new Category() { Name = "AnotherCategory" } };
            var ads = new List<Advertisement>() { ad1, ad2 };

            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5 });

            var adDto = new AdvertisementDto();
            _adConverter.Setup(x => x.ConvertToDto(ad1)).Returns(adDto);

            var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

            CollectionAssert.Contains(result.Advertisements, adDto);
            Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
        }

        [TestCase(null, false, false, "", 10, 10, 200, "", "")]
        public void Filter_Location(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var ad1 = new Advertisement() { Latitude = 10, Longitude = 10 };
            var ad2 = new Advertisement() { Latitude = 50, Longitude = 90 };
            var ads = new List<Advertisement>() { ad1, ad2 };

            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5 });

            var adDto = new AdvertisementDto();
            _adConverter.Setup(x => x.ConvertToDto(ad1)).Returns(adDto);

            var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

            CollectionAssert.Contains(result.Advertisements, adDto);
            Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
        }

        [TestCase(null, false, false, "", 0, 0, 0, "Word", "")]
        public void Filter_Word(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var ad1 = new Advertisement() { Title = "Some word!", Description = "" };
            var ad2 = new Advertisement() { Title = "Something else", Description = "" };
            var ads = new List<Advertisement>() { ad1, ad2 };

            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5 });

            var adDto = new AdvertisementDto();
            _adConverter.Setup(x => x.ConvertToDto(ad1)).Returns(adDto);

            var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

            CollectionAssert.Contains(result.Advertisements, adDto);
            Assert.That(result.Advertisements.Count(), Is.EqualTo(1));
        }

        [TestCase(null, false, false, "", 0, 0, 0, "", "Price")]
        [TestCase(null, false, false, "", 0, 0, 0, "", "Credit")]
        [TestCase(null, false, false, "", 0, 0, 0, "", "Creation")]
        public void SortBy(string advertisementType, bool creditPayment, bool cashPayment, string category, double latitude, double longitude, int distance, string word, string sortBy)
        {
            var ad1 = new Advertisement() { Id = 1, Price = 100, Credit= 0, Creation = DateTime.Now.AddDays(-3) };
            var ad2 = new Advertisement() { Id = 2, Price = 80, Credit= 20, Creation = DateTime.Now };
            var ad3 = new Advertisement() { Id = 3, Price = 10, Credit= 40, Creation = DateTime.Now.AddDays(-4) };
            var ads = new List<Advertisement>() { ad1, ad2, ad3 };

            var adDto1 = new AdvertisementDto() { Id = 1 };
            var adDto2 = new AdvertisementDto() { Id = 2 };
            var adDto3 = new AdvertisementDto() { Id = 3 };


            _advertisementRepository.Setup(x => x.GetAll(userId)).Returns(ads);
            _userRepository.Setup(x => x.GetById(userId)).Returns(new ApplicationUser() { Latitude = 10, Longitude = 5 });

            _adConverter.Setup(x => x.ConvertToDto(ad1)).Returns(adDto1);
            _adConverter.Setup(x => x.ConvertToDto(ad2)).Returns(adDto2);
            _adConverter.Setup(x => x.ConvertToDto(ad3)).Returns(adDto3);

            var result = _sut.GetAdvertisements(advertisementType, creditPayment, cashPayment, category, latitude, longitude, distance, word, sortBy, 0);

            if (sortBy == "Price")
            {
                Assert.That(result.Advertisements.ElementAt(2), Is.EqualTo(adDto1));
                Assert.That(result.Advertisements.ElementAt(1), Is.EqualTo(adDto2));
                Assert.That(result.Advertisements.ElementAt(0), Is.EqualTo(adDto3));
            } else if (sortBy == "Credit")
            {
                Assert.That(result.Advertisements.ElementAt(0), Is.EqualTo(adDto1));
                Assert.That(result.Advertisements.ElementAt(1), Is.EqualTo(adDto2));
                Assert.That(result.Advertisements.ElementAt(2), Is.EqualTo(adDto3));
            } else if (sortBy == "Creation")
            {
                Assert.That(result.Advertisements.ElementAt(1), Is.EqualTo(adDto1));
                Assert.That(result.Advertisements.ElementAt(2), Is.EqualTo(adDto2));
                Assert.That(result.Advertisements.ElementAt(0), Is.EqualTo(adDto3));
            }

            Assert.That(result.Advertisements.Count(), Is.EqualTo(3));
        }
    }
}
