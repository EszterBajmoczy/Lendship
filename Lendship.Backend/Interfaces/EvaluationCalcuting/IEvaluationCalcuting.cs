namespace Lendship.Backend.Interfaces.EvaluationCalcuting
{
    public interface IEvaluationCalcuting
    {
        decimal calculateAdviser(int flexibility, int reliability, int qualityOfProduct);

        decimal calculateLender(int flexibility, int reliability, int qualityAtReturn);

        decimal recalculate(decimal currentEvaluation, int currentCount, decimal evaluation);
    }
}
