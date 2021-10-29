using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Models;

namespace Lendship.Backend.Interfaces.Converters
{
    public interface IMessageConverter
    {
        MessageDto ConvertToDto(Message msg, int conversationId);

        Message ConvertToEntity(MessageDto msgDto, ApplicationUser userFrom);
    }
}
