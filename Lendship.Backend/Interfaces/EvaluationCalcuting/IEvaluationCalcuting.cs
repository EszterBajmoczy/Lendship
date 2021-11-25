namespace Lendship.Backend.Interfaces.EvaluationCalcuting
{
    public interface IEvaluationCalcuting
    {
        int calculateAdviser(int flexibility, int reliability, int qualityOfProduct);

        int calculateLender(int flexibility, int reliability, int qualityAtReturn);

        int recalculate(int currentEvaluation, int currentCount, int evaluation);
    }
}
