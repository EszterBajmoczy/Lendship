using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IConversationRepository
    {
        Conversation GetById(int id);

        void Create(Conversation conversation);
    }
}