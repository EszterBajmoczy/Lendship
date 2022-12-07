using Lendship.Backend.Authentication;
using Lendship.Backend.Interfaces.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Services
{
    public class ReputationCalculatorService: IReputationCalculatorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IEvaluationCalculator _evaluationCalculator;

        private readonly int _m;
        private readonly int _c;
        private readonly double _weight1;
        private readonly double _weight2;
        private readonly double _weight3;

        public ReputationCalculatorService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IEvaluationRepository evaluationRepository,
            IEvaluationCalculator evaluationCalculator)
        {
            _userRepository = userRepository;
            _evaluationRepository = evaluationRepository;
            _evaluationCalculator = evaluationCalculator;

            var config = configuration.GetSection("Reputation");
            _m = config.GetValue("m", 5);
            _c = config.GetValue("C", 5);
            _weight1 = config.GetValue("Weight1", 0.8);
            _weight2 = config.GetValue("Weight2", 0.6);
            _weight3 = config.GetValue("Weight3", 0.4);

        }

        public ReputationCalculatorService(
            IUserRepository userRepository,
            IEvaluationRepository evaluationRepository,
            IEvaluationCalculator evaluationCalculator,
            int m,
            int c,
            double weight1,
            double weight2,
            double weight3)
        {
            _userRepository = userRepository;
            _evaluationRepository = evaluationRepository;
            _evaluationCalculator = evaluationCalculator;

            _m = m;
            _c = c;
            _weight1 = weight1;
            _weight2 = weight2;
            _weight3 = weight3;

        }

        public void RecalculateAdvertiserReputationForUser(ApplicationUser user)
        {
            var evaluationsAsAdvertiser = _evaluationRepository.GetAdvertiserEvaluationsByUser(user.Id);

            var oneYear = DateTime.Now.AddYears(-1);
            var twoYear = DateTime.Now.AddYears(-2);
            var threeYear = DateTime.Now.AddYears(-3);

            user.Evaluation.AdvertiserFlexibility = Calculate(evaluationsAsAdvertiser.Where(e => e.Creation >= oneYear).Select(e => e.Flexibility),
                evaluationsAsAdvertiser.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Flexibility),
                evaluationsAsAdvertiser.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Flexibility),
                evaluationsAsAdvertiser.Where(e => e.Creation < threeYear).Select(e => e.Flexibility));
            user.Evaluation.AdvertiserReliability = Calculate(evaluationsAsAdvertiser.Where(e => e.Creation >= oneYear).Select(e => e.Reliability),
                evaluationsAsAdvertiser.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Reliability),
                evaluationsAsAdvertiser.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Reliability),
                evaluationsAsAdvertiser.Where(e => e.Creation < threeYear).Select(e => e.Reliability));
            user.Evaluation.AdvertiserQualityOfProduct = Calculate(evaluationsAsAdvertiser.Where(e => e.Creation >= oneYear).Select(e => e.QualityOfProduct),
                evaluationsAsAdvertiser.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.QualityOfProduct),
                evaluationsAsAdvertiser.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.QualityOfProduct),
                evaluationsAsAdvertiser.Where(e => e.Creation < threeYear).Select(e => e.Reliability));

            user.Evaluation.EvaluationAsAdvertiser = _evaluationCalculator.calculateAdviser(user.Evaluation.AdvertiserFlexibility, user.Evaluation.AdvertiserReliability, user.Evaluation.AdvertiserQualityOfProduct);
            user.Evaluation.EvaluationAsAdvertiserCount = evaluationsAsAdvertiser.Count();

            _userRepository.Update(user);
        }

        public void RecalculateLenderReputationForUser(ApplicationUser user)
        {
            var oneYear = DateTime.Now.AddYears(-1);
            var twoYear = DateTime.Now.AddYears(-2);
            var threeYear = DateTime.Now.AddYears(-3);

            var evaluationsAsLender = _evaluationRepository.GetLenderEvaluationsByUser(user.Id);

            user.Evaluation.LenderFlexibility = Calculate(evaluationsAsLender.Where(e => e.Creation >= oneYear).Select(e => e.Flexibility),
                evaluationsAsLender.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Flexibility),
                evaluationsAsLender.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Flexibility),
                evaluationsAsLender.Where(e => e.Creation < threeYear).Select(e => e.Flexibility));
            user.Evaluation.LenderReliability = Calculate(evaluationsAsLender.Where(e => e.Creation >= oneYear).Select(e => e.Reliability),
                evaluationsAsLender.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Reliability),
                evaluationsAsLender.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Reliability),
                evaluationsAsLender.Where(e => e.Creation < threeYear).Select(e => e.Reliability));
            user.Evaluation.LenderQualityAtReturn = Calculate(evaluationsAsLender.Where(e => e.Creation >= oneYear).Select(e => e.QualityAtReturn),
                evaluationsAsLender.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.QualityAtReturn),
                evaluationsAsLender.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.QualityAtReturn),
                evaluationsAsLender.Where(e => e.Creation < threeYear).Select(e => e.QualityAtReturn));

            user.Evaluation.EvaluationAsLender = _evaluationCalculator.calculateLender(user.Evaluation.LenderFlexibility, user.Evaluation.LenderReliability, user.Evaluation.LenderQualityAtReturn);
            user.Evaluation.EvaluationAsLenderCount = evaluationsAsLender.Count();

            _userRepository.Update(user);
        }

        double Calculate(IEnumerable<int> evaluationGroup1, IEnumerable<int> evaluationGroup2, IEnumerable<int> evaluationGroup3, IEnumerable<int> evaluationGroup4)
        {
            return (_c * _m + evaluationGroup1.Sum() + evaluationGroup2.Sum() * _weight1 + evaluationGroup3.Sum() * _weight2 + evaluationGroup4.Sum() * _weight3) 
                / (_c + evaluationGroup1.Count() + evaluationGroup2.Count() * _weight1 + evaluationGroup3.Count() * _weight2 + evaluationGroup4.Count() * _weight3);
        }
    }
}
