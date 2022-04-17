using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Converters
{
    public class ConversationConverter : IConversationConverter
    {
        private IUserConverter _userConverter;
        public ConversationConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public ConversationDto ConvertToDto(Conversation conversation, List<ApplicationUser> users, bool hasNewMessage)
        {
            var userDtos = new List<UserDto>();
            
            foreach(var user in users)
            {
                //TODO evaulation
                var userDto = _userConverter.ConvertToDto(user);
                userDtos.Add(userDto);
            }

            return new ConversationDto()
            {
                Id = conversation.Id,
                AdvertisementId = conversation.Advertisement.Id,
                ConversationName = conversation.Name,
                Users = userDtos,
                HasNewMessage = hasNewMessage
            };
        }

        public Conversation ConvertToEntity(ConversationDto conversationDto, Advertisement advertisement, List<Message> msgs)
        {
            var users = conversationDto
                .Users
                .Where(x => x.Id.HasValue)
                .ToList();

            return new Conversation()
            {
                Id = conversationDto.Id ?? 0,
                Advertisement = advertisement,
                Name = conversationDto.ConversationName,
                Messages = msgs
            };
        }
    }
}
