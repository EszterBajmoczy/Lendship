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
    }
}
