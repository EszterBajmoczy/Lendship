using System;

namespace Lendship.Backend.Exceptions
{
    public class EvaluationNotAllowed : Exception
    {
        public EvaluationNotAllowed(string message) : base(message)
        {
        }
    }
}
