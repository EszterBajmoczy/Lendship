using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IConversationConverter
    {
        ConversationDto ConvertToDto(Conversation conversation, List<ApplicationUser> users, bool hasNewMessage);

        Conversation ConvertToEntity(ConversationDto conversationDto, Advertisement advertisement, List<Message> msgs);
    }
}
