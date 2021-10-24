using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;

namespace Lendship.Backend.Converters
{
    public class EvaluationAdvertiserConverter : IEvaluationAdvertiserConverter
    {
        private readonly IUserConverter _userConverter;

        public EvaluationAdvertiserConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public EvaluationAdvertiserDto ConvertToDto(EvaluationAdvertiser evaluation)
        {
            //TODO evaluations as lender, advertiser
            //TODO if it is anonymus, should client get the UserFrom or rather not
            return new EvaluationAdvertiserDto
            {
                Id = evaluation.Id,
                UserFrom = _userConverter.ConvertToDto(evaluation.UserFrom, 0, 2),
                AdvertisementId = evaluation.Advertisement.Id,
                Flexibility = evaluation.Flexibility,
                Reliability = evaluation.Reliability,
                QualityOfProduct = evaluation.QualityOfProduct,
                Comment = evaluation.Comment,
                IsAnonymous = evaluation.IsAnonymous,
                Creation = evaluation.Creation
            };
        }

        public EvaluationAdvertiser ConvertToEntity(EvaluationAdvertiserDto evaluation, ApplicationUser userFrom, ApplicationUser userTo, Advertisement advertisement)
        {
            return new EvaluationAdvertiser
            {
                Id = evaluation.Id ?? 0,
                UserFrom = userFrom,
                UserTo = userTo,
                Advertisement = advertisement,
                Flexibility = evaluation.Flexibility,
                Reliability = evaluation.Reliability,
                QualityOfProduct = evaluation.QualityOfProduct,
                Comment = evaluation.Comment,
                IsAnonymous = evaluation.IsAnonymous,
                Creation = evaluation.Creation ?? DateTime.Now
            };
        }
    }
}
