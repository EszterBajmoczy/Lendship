using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IEvaluationAdvertiserConverter
    {
        EvaluationAdvertiserDto ConvertToDto(EvaluationAdvertiser evaluation);

        EvaluationAdvertiser ConvertToEntity(EvaluationAdvertiserDto evaluation, ApplicationUser userFrom, ApplicationUser userTo, Advertisement advertisement);
    }
}
