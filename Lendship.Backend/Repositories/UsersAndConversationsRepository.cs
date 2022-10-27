using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class UsersAndConversationsRepository : IUsersAndConversationsRepository
    {
        private readonly LendshipDbContext _dbContext;

        public UsersAndConversationsRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(int conversationId, string firstUserId, string secondUserId)
        {
            var newRelationFirst = new UsersAndConversations()
            {
                ConversationId = conversationId,
                UserId = firstUserId
            };

            var newRelationSecond = new UsersAndConversations()
            {
                ConversationId = conversationId,
                UserId = secondUserId
            };

            _dbContext.UsersAndConversations.Add(newRelationFirst);
            _dbContext.UsersAndConversations.Add(newRelationSecond);
            _dbContext.SaveChanges();
        }

        public IEnumerable<UsersAndConversations> Get(string userId)
        {
            return _dbContext.UsersAndConversations
                            .Where(x => x.UserId == userId)
                            .Include(u => u.User)
                            .Include(u => u.Conversation)
                            .Include(u => u.Conversation.Advertisement)
                            .Include(u => u.Conversation.Messages)
                            .OrderByDescending(x => x.Conversation.Messages.OrderByDescending(m => m.Date).FirstOrDefault().Date);
        }

        IEnumerable<UsersAndConversations> IUsersAndConversationsRepository.GetById(int conversationId, string userId)
        {
            return _dbContext.UsersAndConversations
                            .Include(u => u.User)
                            .Where(x => x.ConversationId == conversationId && x.UserId != userId);
        }

        public int GetNewMessagesCount(string userId)
        {
            return _dbContext.UsersAndConversations
                            .Include(u => u.Conversation)
                            .Include(u => u.Conversation.Messages)
                            .Include(u => u.Conversation.Advertisement)
                            .Where(u => u.UserId == userId && u.Conversation.Messages.Any(m => m.New))
                            .Count();
        }
    }
}
