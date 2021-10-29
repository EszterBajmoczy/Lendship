using System;

namespace Lendship.Backend.Exceptions
{
    public class ConversationNotFoundException : Exception
    {
        public ConversationNotFoundException(string message) : base(message)
        {
        }
    }
}
