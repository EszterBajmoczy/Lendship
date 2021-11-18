using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Exceptions
{
    public class MissingTokenException : Exception
    {
        public MissingTokenException(string message) : base(message)
        {
        }
    }
}
