﻿using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;

namespace Lendship.Backend.Converters
{
    public class UserConverter : IUserConverter
    {
        public UserDto ConvertToDto(ApplicationUser user)
        {
            return new UserDto
            {
                Id = new Guid(user.Id),
                Name = user.UserName,
                Email = user.Email,
                EvaluationAsAdvertiser = Math.Round(user.Evaluation.EvaluationAsAdvertiser, 1),
                EvaluationAsAdvertiserCount = user.Evaluation.EvaluationAsAdvertiserCount,
                EvaluationAsLender = Math.Round(user.Evaluation.EvaluationAsLender, 1),
                EvaluationAsLenderCount = user.Evaluation.EvaluationAsLenderCount,
                Image = user.ImageLocation
            };
        }

        public UserDetailsDto ConvertToUserDetailsDto(ApplicationUser user)
        {
            return new UserDetailsDto()
            {
                Id = new Guid(user.Id),
                Name = user.UserName,
                Email = user.Email,
                EmailNotification = user.EmailNotificationsEnabled,
                Credit = user.Credit,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Location = user.Location,
                EvaluationAsAdvertiser = Math.Round(user.Evaluation.EvaluationAsAdvertiser, 1),
                EvaluationAsAdvertiserCount = user.Evaluation.EvaluationAsAdvertiserCount,
                EvaluationAsLender = Math.Round(user.Evaluation.EvaluationAsLender, 1),
                EvaluationAsLenderCount = user.Evaluation.EvaluationAsLenderCount,
                Registration = user.Registration,
                Image = user.ImageLocation,
                Evaluation = user.Evaluation != null ? ConvertEvaluationComputedToDto(user.Evaluation) : null
            };
        }

        private EvaluationComputedDto ConvertEvaluationComputedToDto(EvaluationComputed evaluation)
        {
            return new EvaluationComputedDto()
            {
                AdvertiserFlexibility = Math.Round(evaluation.AdvertiserFlexibility, 1),
                AdvertiserReliability = Math.Round(evaluation.AdvertiserReliability, 1),
                AdvertiserQualityOfProduct = Math.Round(evaluation.AdvertiserQualityOfProduct, 1),
                LenderFlexibility = Math.Round(evaluation.LenderFlexibility, 1),
                LenderReliability = Math.Round(evaluation.LenderReliability, 1),
                LenderQualityAtReturn = Math.Round(evaluation.LenderQualityAtReturn, 1)
            };
        }
    }
}
