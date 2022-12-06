using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using Microsoft.EntityFrameworkCore;
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

        public void DeleteByAdvertisementId(int advertisementId)
        {
            var conversations = _dbContext.Conversation
                                    .Include(x => x.Advertisement)
                                    .Where(x => x.Advertisement.Id == advertisementId);
            _dbContext.Conversation.RemoveRange(conversations);
            _dbContext.SaveChanges();
        }
    }
}
