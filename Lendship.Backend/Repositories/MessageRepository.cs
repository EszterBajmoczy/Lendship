using Lendship.Backend.Authentication;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly LendshipDbContext _dbContext;

        public MessageRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Message> GetByConversation(int conversationId)
        {
            return _dbContext.Messages
                        .Where(m => m.ConversationId == conversationId)
                        .Include(m => m.UserFrom);
        }

        public void SetMessagesSeen(int conversationId)
        {
            var messages = _dbContext.Messages
            .Where(m => m.ConversationId == conversationId && m.New);

            foreach (var msg in messages)
            {
                msg.New = false;
            }

            _dbContext.SaveChanges();
        }

        public bool HasNewMessage(int conversationId)
        {
            return _dbContext.Messages
                    .Where(m => m.ConversationId == conversationId && m.New).Count() != 0;
        }
    }
}
