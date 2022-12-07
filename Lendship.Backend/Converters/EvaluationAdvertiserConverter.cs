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
            return new EvaluationAdvertiserDto
            {
                Id = evaluation.Id,
                UserFrom = evaluation.IsAnonymous ? null : _userConverter.ConvertToDto(evaluation.UserFrom),
                AdvertisementId = evaluation.AdvertisementId,
                ReservationId = evaluation.ReservationId,
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
                AdvertisementId = advertisement.Id,
                ReservationId = evaluation.ReservationId,
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
