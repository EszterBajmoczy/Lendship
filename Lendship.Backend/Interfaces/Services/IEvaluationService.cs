using Lendship.Backend.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IEvaluationService
    {
        IEnumerable<EvaluationAdvertiserDto> GetAdvertiserEvaluations(string userId);

        void CreateAdvertiserEvaluation(EvaluationAdvertiserDto evaluation, string userId);

        IEnumerable<EvaluationLenderDto> GetLenderEvaluations(string userId);

        void CreateLenderEvaluation(EvaluationLenderDto evaluation, string userToId);
    }
}
