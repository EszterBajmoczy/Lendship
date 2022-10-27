using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IEvaluationRepository
    {
        void Create(EvaluationAdvertiser evaluation);

        void Create(EvaluationLender evaluation);

        IEnumerable<EvaluationAdvertiser> GetAdvertiserByUser(string userId);

        IEnumerable<EvaluationLender> GetLenderByUser(string userId);

        IEnumerable<EvaluationAdvertiser> GetAdvertiserEvaluationsByUser(string userId);

        IEnumerable<EvaluationLender> GetLenderEvaluationsByUser(string userId);

    }
}