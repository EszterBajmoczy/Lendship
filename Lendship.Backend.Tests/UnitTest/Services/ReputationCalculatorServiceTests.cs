using Lendship.Backend.Authentication;
using Lendship.Backend.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Lendship.Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
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
    internal class ReputationCalculatorServiceTests
    {
        private IReputationCalculatorService _sut;

        private Mock<IUserRepository> _userRepository;
        private Mock<IEvaluationRepository> _evaluationRepository;
        private Mock<IEvaluationCalculator> _evaluationCalculator;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IUserRepository>();
            _evaluationRepository = new Mock<IEvaluationRepository>();
            _evaluationCalculator = new Mock<IEvaluationCalculator>();

            _sut = new ReputationCalculatorService(
                _userRepository.Object,
                _evaluationRepository.Object,
                _evaluationCalculator.Object,
                3,
                10,
                0.8,
                0.6,
                0.4);
        }

        [TestCase]
        public void Recalculate_Advertiser_Should_Call_Repository_For_Evaluations()
        {
            var user = new ApplicationUser() { Id = "userId" };
            _evaluationRepository.Setup(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>())).Returns(new List<EvaluationAdvertiser>());

            _sut.RecalculateAdvertiserReputationForUser(user);

            _evaluationRepository.Verify(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void Recalculate_Lender_Should_Call_Repository_For_Evaluations()
        {
            var user = new ApplicationUser() { Id = "userId" };
            _evaluationRepository.Setup(x => x.GetLenderEvaluationsByUser(It.IsAny<string>())).Returns(new List<EvaluationLender>());

            _sut.RecalculateLenderReputationForUser(user);

            _evaluationRepository.Verify(x => x.GetLenderEvaluationsByUser(It.IsAny<string>()), Times.Once);
        }

        [TestCase]
        public void Recalculate_Advertiser_Should_Be_Less_Than_Normal_Average_Without_Aging()
        {
            var user = new ApplicationUser() { Id = "userId" };
            var ev1 = new EvaluationAdvertiser() { Flexibility = 5, QualityOfProduct = 5, Reliability = 2, Creation = DateTime.Now };
            var ev2 = new EvaluationAdvertiser() { Flexibility = 5, QualityOfProduct = 3, Reliability = 5, Creation = DateTime.Now };
            var ev3 = new EvaluationAdvertiser() { Flexibility = 5, QualityOfProduct = 4, Reliability = 5, Creation = DateTime.Now };

            double average1 = 0;
            double average2 = 0;
            double average3 = 0;

            var evaluations = new List<EvaluationAdvertiser>() { ev1, ev2, ev3};

            _evaluationRepository.Setup(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateAdvertiser(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                    {
                        average1 = a;
                        average2 = b;
                        average3 = c;
                    });

            _sut.RecalculateAdvertiserReputationForUser(user);

            var flexibilityAverageNormal = Average(ev1.Flexibility, ev2.Flexibility, ev3.Flexibility);
            Assert.IsTrue(average1 <= flexibilityAverageNormal);

            var qualityOfProductAverageNormal = Average(ev1.QualityOfProduct, ev2.QualityOfProduct, ev3.QualityOfProduct);
            Assert.IsTrue(average2 <= qualityOfProductAverageNormal);

            var reliabilityAverageNormal = Average(ev1.Reliability, ev2.Reliability, ev3.Reliability);
            Assert.IsTrue(average3 <= reliabilityAverageNormal);
        }

        [TestCase]
        public void Recalculate_Advertiser_Should_Be_More_Than_Normal_Average_Without_Aging()
        {
            var user = new ApplicationUser() { Id = "userId" };
            var ev1 = new EvaluationAdvertiser() { Flexibility = 2, QualityOfProduct = 1, Reliability = 2, Creation = DateTime.Now };
            var ev2 = new EvaluationAdvertiser() { Flexibility = 1, QualityOfProduct = 3, Reliability = 3, Creation = DateTime.Now };
            var ev3 = new EvaluationAdvertiser() { Flexibility = 2, QualityOfProduct = 2, Reliability = 3, Creation = DateTime.Now };

            double average1 = 0;
            double average2 = 0;
            double average3 = 0;

            var evaluations = new List<EvaluationAdvertiser>() { ev1, ev2, ev3 };

            _evaluationRepository.Setup(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateAdvertiser(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    average1 = a;
                    average2 = b;
                    average3 = c;
                });

            _sut.RecalculateAdvertiserReputationForUser(user);

            var flexibilityAverageNormal = Average(ev1.Flexibility, ev2.Flexibility, ev3.Flexibility);
            Assert.IsTrue(average1 >= flexibilityAverageNormal);

            var qualityOfProductAverageNormal = Average(ev1.QualityOfProduct, ev2.QualityOfProduct, ev3.QualityOfProduct);
            Assert.IsTrue(average2 >= qualityOfProductAverageNormal);

            var reliabilityAverageNormal = Average(ev1.Reliability, ev2.Reliability, ev3.Reliability);
            Assert.IsTrue(average3 >= reliabilityAverageNormal);
        }

        [TestCase]
        public void Recalculate_Lender_Should_Be_Less_Than_Normal_Average_Without_Aging()
        {
            var user = new ApplicationUser() { Id = "userId" };
            var ev1 = new EvaluationLender() { Flexibility = 5, QualityAtReturn = 5, Reliability = 2, Creation = DateTime.Now };
            var ev2 = new EvaluationLender() { Flexibility = 5, QualityAtReturn = 3, Reliability = 5, Creation = DateTime.Now };
            var ev3 = new EvaluationLender() { Flexibility = 5, QualityAtReturn = 4, Reliability = 5, Creation = DateTime.Now };

            double average1 = 0;
            double average2 = 0;
            double average3 = 0;

            var evaluations = new List<EvaluationLender>() { ev1, ev2, ev3 };

            _evaluationRepository.Setup(x => x.GetLenderEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateLender(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    average1 = a;
                    average2 = b;
                    average3 = c;
                });

            _sut.RecalculateLenderReputationForUser(user);

            var flexibilityAverageNormal = Average(ev1.Flexibility, ev2.Flexibility, ev3.Flexibility);
            Assert.IsTrue(average1 <= flexibilityAverageNormal);

            var qualityOfProductAverageNormal = Average(ev1.QualityAtReturn, ev2.QualityAtReturn, ev3.QualityAtReturn);
            Assert.IsTrue(average2 <= qualityOfProductAverageNormal);

            var reliabilityAverageNormal = Average(ev1.Reliability, ev2.Reliability, ev3.Reliability);
            Assert.IsTrue(average3 <= reliabilityAverageNormal);
        }

        [TestCase]
        public void Recalculate_Lender_Should_Be_More_Than_Normal_Average_Without_Aging()
        {
            var user = new ApplicationUser() { Id = "userId" };
            var ev1 = new EvaluationLender() { Flexibility = 2, QualityAtReturn = 1, Reliability = 2, Creation = DateTime.Now };
            var ev2 = new EvaluationLender() { Flexibility = 1, QualityAtReturn = 3, Reliability = 3, Creation = DateTime.Now };
            var ev3 = new EvaluationLender() { Flexibility = 2, QualityAtReturn = 2, Reliability = 3, Creation = DateTime.Now };

            double average1 = 0;
            double average2 = 0;
            double average3 = 0;

            var evaluations = new List<EvaluationLender>() { ev1, ev2, ev3 };

            _evaluationRepository.Setup(x => x.GetLenderEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateLender(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    average1 = a;
                    average2 = b;
                    average3 = c;
                });

            _sut.RecalculateLenderReputationForUser(user);

            var flexibilityAverageNormal = Average(ev1.Flexibility, ev2.Flexibility, ev3.Flexibility);
            Assert.IsTrue(average1 >= flexibilityAverageNormal);

            var qualityOfProductAverageNormal = Average(ev1.QualityAtReturn, ev2.QualityAtReturn, ev3.QualityAtReturn);
            Assert.IsTrue(average2 >= qualityOfProductAverageNormal);

            var reliabilityAverageNormal = Average(ev1.Reliability, ev2.Reliability, ev3.Reliability);
            Assert.IsTrue(average3 >= reliabilityAverageNormal);
        }

        [TestCase]
        public void Recalculate_Advertiser_Aging()
        {
            var user = new ApplicationUser() { Id = "userId" };
            var ev1 = new EvaluationAdvertiser() { Flexibility = 1, QualityOfProduct = 1, Reliability = 1, Creation = DateTime.Now };
            var ev2 = new EvaluationAdvertiser() { Flexibility = 5, QualityOfProduct = 5, Reliability = 5, Creation = DateTime.Now };
            var evaluations = new List<EvaluationAdvertiser>() { ev1, ev2 };
            double average1 = 0;
            double average2 = 0;
            double average3 = 0;

            _evaluationRepository.Setup(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateAdvertiser(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    average1 = a;
                    average2 = b;
                    average3 = c;
                });

            _sut.RecalculateAdvertiserReputationForUser(user);

            var evAging1 = new EvaluationAdvertiser() { Flexibility = 1, QualityOfProduct = 1, Reliability = 1, Creation = DateTime.Now };
            var evAging2 = new EvaluationAdvertiser() { Flexibility = 5, QualityOfProduct = 5, Reliability = 5, Creation = DateTime.Now.AddYears(-1).AddDays(-1) };
            var evaluationsAging = new List<EvaluationAdvertiser>() { evAging1, evAging2 };
            double averageAging1 = 0;
            double averageAging2 = 0;
            double averageAging3 = 0;

            _evaluationRepository.Setup(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>())).Returns(evaluationsAging);
            _evaluationCalculator.Setup(x => x.CalculateAdvertiser(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    averageAging1 = a;
                    averageAging2 = b;
                    averageAging3 = c;
                });

            _sut.RecalculateAdvertiserReputationForUser(user);

            Assert.IsTrue(averageAging1 < average1);
            Assert.IsTrue(averageAging2 < average2);
            Assert.IsTrue(averageAging3 < average3);
        }

        [TestCase]
        public void Recalculate_Lender_Aging()
        {
            var user = new ApplicationUser() { Id = "userId" };
            var ev1 = new EvaluationLender() { Flexibility = 5, QualityAtReturn = 5, Reliability = 5, Creation = DateTime.Now };
            var ev2 = new EvaluationLender() { Flexibility = 1, QualityAtReturn = 1, Reliability = 1, Creation = DateTime.Now };
            var evaluations = new List<EvaluationLender>() { ev1, ev2 };
            double average1 = 0;
            double average2 = 0;
            double average3 = 0;

            _evaluationRepository.Setup(x => x.GetLenderEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateLender(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    average1 = a;
                    average2 = b;
                    average3 = c;
                });

            _sut.RecalculateLenderReputationForUser(user);

            var evAging1 = new EvaluationLender() { Flexibility = 5, QualityAtReturn = 5, Reliability = 5, Creation = DateTime.Now };
            var evAging2 = new EvaluationLender() { Flexibility = 1, QualityAtReturn = 1, Reliability = 1, Creation = DateTime.Now.AddYears(-1).AddDays(-1) };
            var evaluationsAging = new List<EvaluationLender>() { evAging1, evAging2 };
            double averageAging1 = 0;
            double averageAging2 = 0;
            double averageAging3 = 0;

            _evaluationRepository.Setup(x => x.GetLenderEvaluationsByUser(It.IsAny<string>())).Returns(evaluationsAging);
            _evaluationCalculator.Setup(x => x.CalculateLender(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
                .Callback((double a, double b, double c) =>
                {
                    averageAging1 = a;
                    averageAging2 = b;
                    averageAging3 = c;
                });

            _sut.RecalculateLenderReputationForUser(user);

            Assert.IsTrue(averageAging1 > average1);
            Assert.IsTrue(averageAging2 > average2);
            Assert.IsTrue(averageAging3 > average3);
        }

        [TestCase(4.2)]
        public void Recalculate_Advertiser_Update_UserData(double expectedEvaluation)
        {
            var user = new ApplicationUser() { Id = "userId" };
            var evaluations = new List<EvaluationAdvertiser>() { new EvaluationAdvertiser(), new EvaluationAdvertiser() };
            ApplicationUser updatedUser = null;

            _evaluationRepository.Setup(x => x.GetAdvertiserEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateAdvertiser(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(expectedEvaluation);
            _userRepository.Setup(x => x.Update(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser user) =>
                {
                    updatedUser = user;
                });

            _sut.RecalculateAdvertiserReputationForUser(user);

            Assert.That(updatedUser.EvaluationAsAdvertiser, Is.EqualTo(expectedEvaluation));
            Assert.That(updatedUser.EvaluationAsAdvertiserCount, Is.EqualTo(evaluations.Count()));
        }

        [TestCase(4.2)]
        public void Recalculate_Lender_Update_UserData(double expectedEvaluation)
        {
            var user = new ApplicationUser() { Id = "userId" };
            var evaluations = new List<EvaluationLender>() { new EvaluationLender(), new EvaluationLender() };
            ApplicationUser updatedUser = null;

            _evaluationRepository.Setup(x => x.GetLenderEvaluationsByUser(It.IsAny<string>())).Returns(evaluations);
            _evaluationCalculator.Setup(x => x.CalculateLender(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(expectedEvaluation);
            _userRepository.Setup(x => x.Update(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser user) =>
                {
                    updatedUser = user;
                });

            _sut.RecalculateLenderReputationForUser(user);

            Assert.That(updatedUser.EvaluationAsLender, Is.EqualTo(expectedEvaluation));
            Assert.That(updatedUser.EvaluationAsLenderCount, Is.EqualTo(evaluations.Count()));
        }

        private double Average(int a, int b, int c)
        {
            return (a + b + c) / 3;
        }

    }
}
