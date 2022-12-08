namespace Lendship.Backend.Interfaces.EvaluationCalcuting
{
    public interface IEvaluationCalculator
    {
        double CalculateAdvertiser(double flexibility, double reliability, double qualityOfProduct);

        double CalculateLender(double flexibility, double reliability, double qualityAtReturn);
    }
}
