namespace Lendship.Backend.Interfaces.EvaluationCalcuting
{
    public interface IEvaluationCalculator
    {
        double calculateAdviser(double flexibility, double reliability, double qualityOfProduct);

        double calculateLender(double flexibility, double reliability, double qualityAtReturn);
    }
}
