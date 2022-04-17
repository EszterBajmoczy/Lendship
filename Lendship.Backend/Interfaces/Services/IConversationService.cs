using Lendship.Backend.DTO;
using System;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface IConversationService
    {
        IEnumerable<ConversationDto> GetAllConversation(string searchString);

        int CreateConversation(ConversationDto conversationDto);

        void CreateMessage(MessageDto messageDto);

        IEnumerable<MessageDto> GetAllMessage(int conversationId);

        int GetNewMessageCount();
    }
}
