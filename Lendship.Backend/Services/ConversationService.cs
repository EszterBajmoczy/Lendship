using Lendship.Backend.Authentication;
using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lendship.Backend.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly IConversationConverter _conversationConverter;
        private readonly IMessageConverter _messageConverter;

        public ConversationService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            var userConverter = new UserConverter();

            _conversationConverter = new ConversationConverter(userConverter);
            _messageConverter = new MessageConverter(userConverter);
        }

        public void CreateConversation(ConversationDto conversationDto)
        {
            throw new NotImplementedException();
        }

        public void CreateMessage(MessageDto messageDto, int conversationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ConversationDto> GetAllConversation(string searchString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MessageDto> GetAllMessage(int conversationId)
        {
            throw new NotImplementedException();
        }

        public void UpdateReservation(ReservationDto reservation)
        {
            throw new NotImplementedException();
        }
    }
}
