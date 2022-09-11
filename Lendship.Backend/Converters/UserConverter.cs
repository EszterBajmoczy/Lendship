using Lendship.Backend.Authentication;
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
                EvaluationAsAdvertiserCount = user.EvaluationAsAdvertiserCount,
                EvaluationAsLender = user.EvaluationAsLender,
                EvaluationAsLenderCount = user.EvaluationAsLenderCount,
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
                EvaluationAsAdvertiser = user.EvaluationAsAdvertiser,
                EvaluationAsAdvertiserCount = user.EvaluationAsAdvertiserCount,
                EvaluationAsLender = user.EvaluationAsLender,
                EvaluationAsLenderCount = user.EvaluationAsLenderCount,
                Registration = user.Registration,
                Image = user.ImageLocation
            };
        }
    }
}
