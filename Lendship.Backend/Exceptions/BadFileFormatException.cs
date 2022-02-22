using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Exceptions
{
    public class BadFileFormatException : Exception
    {
        public BadFileFormatException(string message) : base(message)
        {
        }
    }
}
