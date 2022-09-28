using Lendship.Backend.Authentication;
using Lendship.Backend.Interfaces.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Services
{
    public class ReputationCalculatorService: IReputationCalculatorService
    {
        private readonly LendshipDbContext _dbContext;
        private readonly IEvaluationCalculator _evaluationCalculator;

        private readonly int m;
        private readonly int c;
        private readonly double weight1;
        private readonly double weight2;
        private readonly double weight3;

        public ReputationCalculatorService(LendshipDbContext dbContext, IConfiguration configuration, IEvaluationCalculator evaluationCalculator)
        {
            _dbContext = dbContext;
            _evaluationCalculator = evaluationCalculator;

            var config = configuration.GetSection("Reputation");
            m = config.GetValue("m", 5);
            c = config.GetValue("C", 5);
            weight1 = config.GetValue("Weight1", 0.8);
            weight2 = config.GetValue("Weight2", 0.6);
            weight3 = config.GetValue("Weight3", 0.4);

        }

        public void RecalculateAdvertiserReputationForUser(ApplicationUser user)
        {
            var evaluationsAsAdvertiser = _dbContext.EvaluationAdvertisers
                                                        .Include(e => e.UserTo)
                                                        .Where(e => e.UserTo.Id == user.Id);
            var oneYear = DateTime.Now.AddYears(-1);
            var twoYear = DateTime.Now.AddYears(-2);
            var threeYear = DateTime.Now.AddYears(-3);

            user.AdvertiserFlexibility = Calculate(evaluationsAsAdvertiser.Where(e => e.Creation >= oneYear).Select(e => e.Flexibility),
                evaluationsAsAdvertiser.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Flexibility),
                evaluationsAsAdvertiser.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Flexibility),
                evaluationsAsAdvertiser.Where(e => e.Creation < threeYear).Select(e => e.Flexibility));
            user.AdvertiserReliability = Calculate(evaluationsAsAdvertiser.Where(e => e.Creation >= oneYear).Select(e => e.Reliability),
                evaluationsAsAdvertiser.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Reliability),
                evaluationsAsAdvertiser.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Reliability),
                evaluationsAsAdvertiser.Where(e => e.Creation < threeYear).Select(e => e.Reliability));
            user.AdvertiserQualityOfProduct = Calculate(evaluationsAsAdvertiser.Where(e => e.Creation >= oneYear).Select(e => e.QualityOfProduct),
                evaluationsAsAdvertiser.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.QualityOfProduct),
                evaluationsAsAdvertiser.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.QualityOfProduct),
                evaluationsAsAdvertiser.Where(e => e.Creation < threeYear).Select(e => e.Reliability));

            user.EvaluationAsAdvertiser = _evaluationCalculator.calculateAdviser(user.AdvertiserFlexibility, user.AdvertiserReliability, user.AdvertiserQualityOfProduct);
            user.EvaluationAsAdvertiserCount = evaluationsAsAdvertiser.Count();

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public void RecalculateLenderReputationForUser(ApplicationUser user)
        {
            var oneYear = DateTime.Now.AddYears(-1);
            var twoYear = DateTime.Now.AddYears(-2);
            var threeYear = DateTime.Now.AddYears(-3);

            var evaluationsAsLender = _dbContext.EvaluationLenders
                                                        .Include(e => e.UserTo)
                                                        .Where(e => e.UserTo.Id == user.Id);

            user.LenderFlexibility = Calculate(evaluationsAsLender.Where(e => e.Creation >= oneYear).Select(e => e.Flexibility),
                evaluationsAsLender.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Flexibility),
                evaluationsAsLender.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Flexibility),
                evaluationsAsLender.Where(e => e.Creation < threeYear).Select(e => e.Flexibility));
            user.LenderReliability = Calculate(evaluationsAsLender.Where(e => e.Creation >= oneYear).Select(e => e.Reliability),
                evaluationsAsLender.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.Reliability),
                evaluationsAsLender.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.Reliability),
                evaluationsAsLender.Where(e => e.Creation < threeYear).Select(e => e.Reliability));
            user.LenderQualityAtReturn = Calculate(evaluationsAsLender.Where(e => e.Creation >= oneYear).Select(e => e.QualityAtReturn),
                evaluationsAsLender.Where(e => e.Creation >= twoYear && e.Creation < oneYear).Select(e => e.QualityAtReturn),
                evaluationsAsLender.Where(e => e.Creation >= threeYear && e.Creation < twoYear).Select(e => e.QualityAtReturn),
                evaluationsAsLender.Where(e => e.Creation < threeYear).Select(e => e.QualityAtReturn));

            user.EvaluationAsLender = _evaluationCalculator.calculateLender(user.LenderFlexibility, user.LenderReliability, user.LenderQualityAtReturn);
            user.EvaluationAsLenderCount = evaluationsAsLender.Count();

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        double Calculate(IEnumerable<int> evaluationGroup1, IEnumerable<int> evaluationGroup2, IEnumerable<int> evaluationGroup3, IEnumerable<int> evaluationGroup4)
        {
            return (c * m + evaluationGroup1.Sum() + evaluationGroup2.Sum() * weight1 + evaluationGroup3.Sum() * weight2 + evaluationGroup4.Sum() * weight3) 
                / (c + evaluationGroup1.Count());
        }
    }
}
