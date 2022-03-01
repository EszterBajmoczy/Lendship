using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;

namespace Lendship.Backend.Converters
{
    public class MessageConverter : IMessageConverter
    {
        private IUserConverter _userConverter;
        public MessageConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public MessageDto ConvertToDto(Message msg, int conversationId)
        {
            return new MessageDto()
            {
                Id = msg.Id,
                ConversationId = conversationId,
                UserFrom = _userConverter.ConvertToDto(msg.UserFrom),
                Content = msg.Content,
                New = msg.New,
                Date = msg.Date
            };
        }

        public Message ConvertToEntity(MessageDto msgDto, ApplicationUser userFrom)
        {
            return new Message()
            {
                Id = msgDto.Id ?? 0,
                UserFrom = userFrom,
                ConversationId = msgDto.ConversationId,
                Content = msgDto.Content,
                New = msgDto.New,
                Date = msgDto.Date ?? DateTime.Now
            };
        }
    }
}
