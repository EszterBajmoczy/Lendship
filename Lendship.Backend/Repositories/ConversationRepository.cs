using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly LendshipDbContext _dbContext;

        public ConversationRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Conversation conversation)
        {
            _dbContext.Conversation.Add(conversation);
            _dbContext.SaveChanges();
        }

        public Conversation GetById(int id)
        {
            return _dbContext.Conversation.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
