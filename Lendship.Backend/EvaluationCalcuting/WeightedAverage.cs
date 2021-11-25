using Lendship.Backend.Interfaces.EvaluationCalcuting;

namespace Lendship.Backend.EvaluationCalcuting
{
    public class WeightedAverage : IEvaluationCalcuting
    {
        public int calculateAdviser(int flexibility, int reliability, int qualityOfProduct)
        {
            return (flexibility + reliability * 3 + qualityOfProduct * 3) / 7;
        }

        public int calculateLender(int flexibility, int reliability, int qualityAtReturn)
        {
            return (flexibility + reliability * 3 + qualityAtReturn * 3) / 7;
        }

        public int recalculate(int currentEvaluation, int currentCount, int evaluation)
        {
            var s = currentEvaluation * currentCount;
            return (s + evaluation) / (currentCount + 1);
        }
    }
}
