using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IUsersAndConversationsRepository
    {
        IEnumerable<UsersAndConversations> Get(string userId);

        IEnumerable<UsersAndConversations> GetById(int conversationId, string userId);

        int GetNewMessagesCount(string userId);

        void Create(int conversationId, string firstUserId, string secondUserId);
    }
}