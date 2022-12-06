using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface IConversationRepository
    {
        Conversation GetById(int id);

        void Create(Conversation conversation);

        void DeleteByAdvertisementId(int advertisementId);
    }
}