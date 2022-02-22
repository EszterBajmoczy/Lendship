using System;

namespace Lendship.Backend.Exceptions
{
    public class NoFileException : Exception
    {
        public NoFileException(string message) : base(message)
        {
        }
    }
}
