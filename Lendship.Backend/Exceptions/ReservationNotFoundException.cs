using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Exceptions
{
    public class ReservationNotFoundException : Exception
    {
        public ReservationNotFoundException(string message) : base(message)
        {
        }
    }
}
