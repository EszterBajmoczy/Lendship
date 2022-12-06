using Lendship.Backend.Authentication;
using Lendship.Backend.Converters;
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
using System.Text;
using System.Threading.Tasks;

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

        private Mock<ICategoryService> _categoryService;
        private Mock<IReservationService> _reservationService;
        private Mock<IImageService> _imageService;

        private Mock<IAdvertisementDetailsConverter> _adDetailsConverter;
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

            _categoryService = new Mock<ICategoryService>();
            _reservationService = new Mock<IReservationService>();
            _imageService = new Mock<IImageService>();

            _adDetailsConverter = new Mock<IAdvertisementDetailsConverter>();
            _adConverter = new Mock<IAdvertisementConverter>();
            _availabilityConverter = new Mock<IAvailabilityConverter>();

            _sut = new AdvertisementService(
                _httpContextAccessor.Object,
                _advertisementRepository.Object,
                _userRepository.Object,
                _savedAdvertisementRepository.Object,
                _availabilityRepository.Object,
                _categoryService.Object,
                _reservationService.Object,
                _imageService.Object,
                _adDetailsConverter.Object,
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

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);
            _adDetailsConverter.Setup(x => x.ConvertToDto(ad)).Returns(adDto);

            var result = _sut.GetAdvertisement(1);

            Assert.That(result, Is.EqualTo(adDto));
        }

        [TestCase]
        public void CreateAdvertisement_Should_Call_GetOrCreateCategoryByName()
        {
            var category = new Category() { Name = "TestCategory" };

            var adDto = new AdvertisementDetailsDto()
            {
                Category = category,
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, It.IsAny<ApplicationUser>(), category)).Returns(ad);

            var result = _sut.CreateAdvertisement(adDto);

            _categoryService.Verify(x => x.GetOrCreateCategoryByName(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void CreateAdvertisement_Should_Call_UserRepository()
        {
            var category = new Category() { Name = "TestCategory" };

            var adDto = new AdvertisementDetailsDto()
            {
                Category = category,
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, It.IsAny<ApplicationUser>(), category)).Returns(ad);

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
                Category = category,
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            var result = _sut.CreateAdvertisement(adDto);

            _adDetailsConverter.Verify(x => x.ConvertToEntity(adDto, user, category), Times.Once);
            _advertisementRepository.Verify(x => x.Create(ad), Times.Once);
        }

        [TestCase]
        public void CreateAdvertisement_Should_Return_Id()
        {
            var category = new Category() { Name = "TestCategory" };
            var user = new ApplicationUser();

            var adDto = new AdvertisementDetailsDto()
            {
                Category = category,
                Availabilities = new List<AvailabilityDto>()
            };

            var ad = new Advertisement() { Id = 1 };

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement() { Id = 1 };

            IEnumerable<Availability> addedList = null;

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement() { Id = 1 };

            IEnumerable<Availability> deletedList = null;

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
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

                _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);

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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            _sut.UpdateAdvertisement(adDto);

            _userRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Once);
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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement() { User = new ApplicationUser() { Id = userId } };

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);

            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);

            _sut.UpdateAdvertisement(adDto);

            _adDetailsConverter.Verify(x => x.ConvertToEntity(adDto, user, category), Times.Once);
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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement() 
            { 
                Id = 1,
                User = new ApplicationUser() { Id = userId } 
            };

            IEnumerable<Availability> addedList = null;

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);
            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
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
                Category = category,
                Availabilities = availabilities
            };

            var ad = new Advertisement()
            {
                Id = 1,
                User = new ApplicationUser() { Id = userId }
            };

            IEnumerable<Availability> deletedList = null;

            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ad);
            _categoryService.Setup(x => x.GetOrCreateCategoryByName(It.IsAny<string>())).Returns(category);
            _userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns(user);
            _adDetailsConverter.Setup(x => x.ConvertToEntity(adDto, user, category)).Returns(ad);
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
            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Advertisement());

            _sut.DeleteAdvertisement(advertisementId);

            _imageService.Verify(x => x.DeleteImages(It.IsAny<int>()), Times.Once);
        }

        [TestCase(1)]
        public void DeleteAdvertisement_Should_Delete_Advertisement_And_Call_ReservationService(int advertisementId)
        {
            _advertisementRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Advertisement());

            _sut.DeleteAdvertisement(advertisementId);

            _advertisementRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
            _reservationService.Verify(x => x.RemoveUpcommingReservations(It.IsAny<int>()), Times.Once);
        }

        [TestCase("Service", true, true, "", "", 0, "", "")]
        [TestCase("Property", true, true, "", "", 0, "", "")]
        public void GetAdvertisements_Filter_AdvertisementType(string advertisementType, bool creditPayment, bool cashPayment, string category, string city, int distance, string word, string sortBy)
        {
            var ads = new List<Advertisement>()
            {
                new Advertisement() { Type}
            }

            _advertisementRepository.Setup(x => x.GetAll()).Returns();

            var result = _sut.GetAdvertisements(advertisementType, true, true);

            _advertisementRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
            _reservationService.Verify(x => x.RemoveUpcommingReservations(It.IsAny<int>()), Times.Once);
        }
    }
}
