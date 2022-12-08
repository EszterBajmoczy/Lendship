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
    public class EvaluationLenderConverter : IEvaluationLenderConverter
    {
        private readonly IUserConverter _userConverter;

        public EvaluationLenderConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public EvaluationLenderDto ConvertToDto(EvaluationLender evaluation)
        {
            return new EvaluationLenderDto
            {
                Id = evaluation.Id,
                UserFrom = evaluation.IsAnonymous ? null : _userConverter.ConvertToDto(evaluation.UserFrom),
                AdvertisementId = evaluation.AdvertisementId,
                ReservationId = evaluation.ReservationId,
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
                AdvertisementId = advertisement.Id,
                ReservationId = evaluation.ReservationId,
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
