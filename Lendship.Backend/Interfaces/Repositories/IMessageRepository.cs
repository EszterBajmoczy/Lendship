using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetByConversation(int conversationId);

        void Create(Message message);

        bool HasNewMessage(int conversationId, string signedInUserId);

        void SetMessagesSeen(int conversationId, string signedInUserId);
    }
}