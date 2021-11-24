﻿using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
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
                EvaluationAsAdvertiser = user.EvaluationAsAdvertiser,
                EvaluationAsAdvertiserCount = user.EvaluationAsLenderCount,
                EvaluationAsLender = user.EvaluationAsLender,
                EvaluationAsLenderCount = user.EvaluationAsLenderCount
            };
        }

        public UserDetailsDto ConvertToUserDetaiolsDto(ApplicationUser user)
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
                EvaluationAsAdvertiser = user.EvaluationAsAdvertiser,
                EvaluationAsAdvertiserCount = user.EvaluationAsLenderCount,
                EvaluationAsLender = user.EvaluationAsLender,
                EvaluationAsLenderCount = user.EvaluationAsLenderCount,
                Registration = user.Registration
            };
        }
    }
}
