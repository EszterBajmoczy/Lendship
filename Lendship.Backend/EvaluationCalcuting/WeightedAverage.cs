using Lendship.Backend.Interfaces.EvaluationCalcuting;

namespace Lendship.Backend.EvaluationCalcuting
{
    public class WeightedAverage : IEvaluationCalculator
    {
        public double CalculateAdvertiser(double flexibility, double reliability, double qualityOfProduct)
        {
            return (flexibility + reliability * 3 + qualityOfProduct * 3) / 7;
        }

        public double CalculateLender(double flexibility, double reliability, double qualityAtReturn)
        {
            return (flexibility + reliability * 3 + qualityAtReturn * 3) / 7;
        }
    }
}
