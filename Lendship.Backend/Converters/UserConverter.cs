using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using System;

namespace Lendship.Backend.Converters
{
    public class UserConverter : IUserConverter
    {
        public UserDto ConvertToDto(ApplicationUser user, int evaluationAsLender, int evaluationAsAdvertiser)
        {
            return new UserDto
            {
                Id = new Guid(user.Id),
                Name = user.UserName,
                EvaluationAsAdvertiser = evaluationAsAdvertiser,
                EvaluationAsLender = evaluationAsLender
            };
        }

        public UserDetailsDto ConvertToUserDetaiolsDto(ApplicationUser user, int evaluationAsLender, int evaluationAsAdvertiser)
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
                EvaluationAsAdvertiser = evaluationAsAdvertiser,
                EvaluationAsLender = evaluationAsLender,
                Registration = user.Registration
            };
        }
    }
}
