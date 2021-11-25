﻿using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.EvaluationCalcuting;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly IEvaluationAdvertiserConverter _advertiserConverter;
        private readonly IEvaluationLenderConverter _lenderConverter;
        private readonly IEvaluationCalcuting _calcuting;

        public EvaluationService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext, IEvaluationCalcuting calcuting)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _calcuting = calcuting;

            //TODO inject converters!!
            var userConverter = new UserConverter();
            _advertiserConverter = new EvaluationAdvertiserConverter(userConverter);
            _lenderConverter = new EvaulationLenderConverter(userConverter);
        }

        public void CreateAdvertiserEvaluation(EvaluationAdvertiserDto evaluationDto, string userToId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFrom = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();

            var userTo = _dbContext.Users.Where(x => x.Id == userToId).FirstOrDefault();
            if (userTo == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var advertisement = _dbContext.Advertisements.Where(x => x.Id == evaluationDto.AdvertisementId).FirstOrDefault();
            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            var evaluation = _advertiserConverter.ConvertToEntity(evaluationDto, userFrom, userTo, advertisement);
            _dbContext.EvaluationAdvertisers.Add(evaluation);

            var calculated = _calcuting.calculateAdviser(evaluation.Flexibility, evaluation.Reliability, evaluation.QualityOfProduct);
            var updatedEvaluation = _calcuting.recalculate(userTo.EvaluationAsAdvertiser, userTo.EvaluationAsAdvertiserCount, calculated);

            userTo.EvaluationAsAdvertiser = updatedEvaluation;
            userTo.EvaluationAsAdvertiserCount++;
            _dbContext.Update(userTo);
            _dbContext.SaveChanges();
        }

        public void CreateLenderEvaluation(EvaluationLenderDto evaluationDto, string userToId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFrom = _dbContext.Users.Where(x => x.Id == signedInUserId).FirstOrDefault();

            var userTo = _dbContext.Users.Where(x => x.Id == userToId).FirstOrDefault();
            if (userTo == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var advertisement = _dbContext.Advertisements.Where(x => x.Id == evaluationDto.AdvertisementId).FirstOrDefault();
            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not found.");
            }

            var evaluation = _lenderConverter.ConvertToEntity(evaluationDto, userFrom, userTo, advertisement);
            _dbContext.EvaluationLenders.Add(evaluation);

            var calculated = _calcuting.calculateLender(evaluation.Flexibility, evaluation.Reliability, evaluation.QualityAtReturn);
            var updatedEvaluation = _calcuting.recalculate(userTo.EvaluationAsAdvertiser, userTo.EvaluationAsAdvertiserCount, calculated);

            userTo.EvaluationAsAdvertiser = updatedEvaluation;
            userTo.EvaluationAsAdvertiserCount++;
            _dbContext.Update(userTo);
            _dbContext.SaveChanges();
        }

        public IEnumerable<EvaluationAdvertiserDto> GetAdvertiserEvaluations(string userId)
        {
            var resultList = new List<EvaluationAdvertiserDto>();

            var evaluations = _dbContext.EvaluationAdvertisers
                        .Include(e => e.UserFrom)
                        .Include(e => e.Advertisement)
                        .Where(e => e.UserTo.Id == userId)
                        .ToList();

            foreach (var evaluation in evaluations)
            {
                var dto = _advertiserConverter.ConvertToDto(evaluation);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<EvaluationLenderDto> GetLenderEvaluations(string userId)
        {
            var resultList = new List<EvaluationLenderDto>();

            var evaluations = _dbContext.EvaluationLenders
                        .Include(e => e.UserFrom)
                        .Include(e => e.Advertisement)
                        .Where(e => e.UserTo.Id == userId)
                        .ToList();

            foreach (var evaluation in evaluations)
            {
                var dto = _lenderConverter.ConvertToDto(evaluation);
                resultList.Add(dto);
            }

            return resultList;
        }
    }
}
