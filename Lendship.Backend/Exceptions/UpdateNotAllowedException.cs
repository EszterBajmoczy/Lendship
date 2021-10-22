using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Exceptions
{
    public class UpdateNotAllowedException : Exception
    {
        public UpdateNotAllowedException(string message) : base(message)
        {
        }
    }
}
