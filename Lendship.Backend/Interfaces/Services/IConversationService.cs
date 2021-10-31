using Lendship.Backend.DTO;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IConversationService
    {
        IEnumerable<ConversationDto> GetAllConversation(string searchString);

        void CreateConversation(ConversationDto conversationDto);

        void CreateMessage(MessageDto messageDto);

        IEnumerable<MessageDto> GetAllMessage(int conversationId);
    }
}
