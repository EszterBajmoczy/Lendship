using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IEvaluationLenderConverter
    {
        EvaluationLenderDto ConvertToDto(EvaluationLender evaluation);

        EvaluationLender ConvertToEntity(EvaluationLenderDto evaluation, ApplicationUser UserFrom, ApplicationUser UserTo, Advertisement advertisement);
    }
}
