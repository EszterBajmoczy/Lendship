using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IConversationConverter
    {
        ConversationDto ConvertToDto(Conversation conversation);

        Conversation ConvertToEntity(ConversationDto conversationDto);
    }
}
