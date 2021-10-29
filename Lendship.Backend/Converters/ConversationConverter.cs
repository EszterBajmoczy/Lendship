﻿using Lendship.Backend.Authentication;
using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;
using System;
using System.Collections.Generic;

namespace Lendship.Backend.Converters
{
    public class ConversationConverter : IConversationConverter
    {
        private IUserConverter _userConverter;
        public ConversationConverter(IUserConverter userConverter)
        {
            _userConverter = userConverter;
        }

        public ConversationDto ConvertToDto(Conversation conversation)
        {
            var userDtos = new List<UserDto>();

            foreach(var user in conversation.Users)
            {
                //TODO evaulation
                var userDto = _userConverter.ConvertToDto(user, 0, 1);
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

        public Conversation ConvertToEntity(ConversationDto conversationDto, Advertisement advertisement, List<ApplicationUser> users, List<Message> msgs)
        {
            return new Conversation()
            {
                Id = conversationDto.Id ?? 0,
                Advertisement = advertisement,
                Name = conversationDto.ConversationName,
                Users = users,
                Messages = msgs
            };
        }
    }
}
