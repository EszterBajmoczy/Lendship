using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lendship.Backend.Exceptions
{
    public class CategoryNotExistsException : Exception
    {
        public CategoryNotExistsException(string message) : base(message)
        {
        }
    }
}
