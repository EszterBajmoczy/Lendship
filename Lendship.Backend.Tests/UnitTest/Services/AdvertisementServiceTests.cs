using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Lendship.Backend.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lendship.Backend.Tests.UnitTest.Services
{
    [TestFixture]
    internal class AdvertisementServiceTests
    {
        private IAdvertisementService _sut;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private Mock<LendshipDbContext> _dbContext;
        private Mock<ICategoryService> _categoryService;
        private Mock<INotificationService> _notificationService;
        private Mock<IImageService> _imageService;

        private Mock<IAdvertisementDetailsConverter> _advertisementDetailsConverter;
        private Mock<IAdvertisementConverter> _advertisementConverter;
        private Mock<IAvailabilityConverter> _availabilityConverter;


        [SetUp]
        public void Setup()
        {
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _dbContext = new Mock<LendshipDbContext>();
            _categoryService = new Mock<ICategoryService>();
            _notificationService = new Mock<INotificationService>();
            _imageService = new Mock<IImageService>();

            _advertisementDetailsConverter = new Mock<IAdvertisementDetailsConverter>();
            _advertisementConverter = new Mock<IAdvertisementConverter>();
            _availabilityConverter = new Mock<IAvailabilityConverter>();
            /*
            _sut = new AdvertisementService(
                _contextAccessor.Object, 
                _dbContext.Object, 
                _categoryService.Object, 
                _notificationService.Object,
                _imageService.Object,
                _advertisementDetailsConverter.Object,
                _advertisementConverter.Object,
                _availabilityConverter.Object);*/
        }

        [TestCase]
        public void GetAdvertisement()
        {
            var s = new List<Advertisement>() { new Advertisement() };
            //_dbContext.Setup(x => x.Advertisements).Returns(s);
        }
    }
}
