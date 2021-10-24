using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Converters
{
    public class EvaulationLenderConverter : IEvaluationLenderConverter
    {
        private readonly IUserConverter _userConverter;

        public EvaulationLenderConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public EvaluationLenderDto ConvertToDto(EvaluationLender evaluation)
        {
            //TODO evaluations as lender, advertiser
            //TODO if it is anonymus, should client get the UserFrom or rather not
            return new EvaluationLenderDto
            {
                Id = evaluation.Id,
                UserFrom = _userConverter.ConvertToDto(evaluation.UserFrom, 0, 2),
                AdvertisementId = evaluation.Advertisement.Id,
                Flexibility = evaluation.Flexibility,
                Reliability = evaluation.Reliability,
                QualityAtReturn = evaluation.QualityAtReturn,
                Comment = evaluation.Comment,
                IsAnonymous = evaluation.IsAnonymous,
                Creation = evaluation.Creation
            };
        }

        public EvaluationLender ConvertToEntity(EvaluationLenderDto evaluation, ApplicationUser UserFrom, ApplicationUser UserTo, Advertisement advertisement)
        {
            return new EvaluationLender
            {
                Id = evaluation.Id ?? 0,
                UserFrom = UserFrom,
                UserTo = UserTo,
                Advertisement = advertisement,
                Flexibility = evaluation.Flexibility,
                Reliability = evaluation.Reliability,
                QualityAtReturn = evaluation.QualityAtReturn,
                Comment = evaluation.Comment,
                IsAnonymous = evaluation.IsAnonymous,
                Creation = evaluation.Creation ?? DateTime.Now
            };
        }
    }
}
