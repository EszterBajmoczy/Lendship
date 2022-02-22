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

        public ConversationDto ConvertToDto(Conversation conversation, List<ApplicationUser> users)
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
                Users = userDtos
            };
        }

        public Conversation ConvertToEntity(ConversationDto conversationDto, Advertisement advertisement, List<Message> msgs)
        {
            var userIds = conversationDto
                .Users
                .Where(x => x.Id.HasValue)
                .Select(x => x.Id.GetValueOrDefault())
                .ToList();

            return new Conversation()
            {
                Id = conversationDto.Id ?? 0,
                Advertisement = advertisement,
                Name = conversationDto.ConversationName,
                UserIds = userIds,
                Messages = msgs
            };
        }
    }
}
