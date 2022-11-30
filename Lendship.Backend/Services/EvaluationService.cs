using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdvertisementRepository _advertisementRepository;

        private readonly IReputationCalculatorService _reputationCalculator;

        private readonly IEvaluationAdvertiserConverter _advertiserConverter;
        private readonly IEvaluationLenderConverter _lenderConverter;

        public EvaluationService(
            IHttpContextAccessor httpContextAccessor, 
            IEvaluationRepository evaluationRepository,
            IUserRepository userRepository,
            IAdvertisementRepository advertisementRepository,
            IReputationCalculatorService reputationCalculator,
            IEvaluationAdvertiserConverter evaluationAdvertiserConverter,
            IEvaluationLenderConverter evaulationLenderConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _evaluationRepository = evaluationRepository;
            _userRepository = userRepository;
            _advertisementRepository = advertisementRepository;

            _reputationCalculator = reputationCalculator;

            _advertiserConverter = evaluationAdvertiserConverter;
            _lenderConverter = evaulationLenderConverter;
        }

        public void CreateAdvertiserEvaluation(EvaluationAdvertiserDto evaluationDto, string userToId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFrom = _userRepository.GetById(signedInUserId);
            var userTo = _userRepository.GetById(userToId);

            if (userTo == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var advertisement = _advertisementRepository.GetById(evaluationDto.AdvertisementId, signedInUserId);
            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            var evaluation = _advertiserConverter.ConvertToEntity(evaluationDto, userFrom, userTo, advertisement);
            _evaluationRepository.Create(evaluation);

            _reputationCalculator.RecalculateAdvertiserReputationForUser(userTo);
        }

        public void CreateLenderEvaluation(EvaluationLenderDto evaluationDto, string userToId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFrom = _userRepository.GetById(signedInUserId);
            var userTo = _userRepository.GetById(userToId);

            if (userTo == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var advertisement = _advertisementRepository.GetPlainById(evaluationDto.AdvertisementId, signedInUserId);
            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            var evaluation = _lenderConverter.ConvertToEntity(evaluationDto, userFrom, userTo, advertisement);
            _evaluationRepository.Create(evaluation);

            _reputationCalculator.RecalculateLenderReputationForUser(userTo);
        }

        public IEnumerable<EvaluationAdvertiserDto> GetAdvertiserEvaluations(string userId)
        {
            return _evaluationRepository.GetAdvertiserByUser(userId)
                .Select(x => _advertiserConverter.ConvertToDto(x));
        }

        public IEnumerable<EvaluationLenderDto> GetLenderEvaluations(string userId)
        {
            return _evaluationRepository.GetLenderByUser(userId)
                .Select(x => _lenderConverter.ConvertToDto(x));
        }
    }
}
