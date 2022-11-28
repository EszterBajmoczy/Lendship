using Lendship.Backend.Authentication;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetByConversation(int conversationId);

        void Create(Message message);

        bool HasNewMessage(int conversationId);

        void SetMessagesSeen(int conversationId);
    }
}