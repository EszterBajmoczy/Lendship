using Lendship.Backend.Interfaces.EvaluationCalcuting;

namespace Lendship.Backend.EvaluationCalcuting
{
    public class WeightedAverage : IEvaluationCalculator
    {
        public double calculateAdviser(double flexibility, double reliability, double qualityOfProduct)
        {
            return (flexibility + reliability * 3 + qualityOfProduct * 3) / 7;
        }

        public double calculateLender(double flexibility, double reliability, double qualityAtReturn)
        {
            return (flexibility + reliability * 3 + qualityAtReturn * 3) / 7;
        }
    }
}
