using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Exceptions
{
    public class AdvertisementNotFoundException : Exception
    {
        public AdvertisementNotFoundException(string message) : base(message)
        {
        }
    }
}
