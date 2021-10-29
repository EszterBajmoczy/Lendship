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
            var advertisement = _dbContext.Advertisements.Where(x => x.Id == conversationDto.AdvertisementId).FirstOrDefault();

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not exists.");
            }

            var userIds = conversationDto.Users.Select(u => u.Id);
            var users = _dbContext.Users.Where(x => userIds.Contains(new Guid(x.Id))).ToList();

            var conversation = _conversationConverter.ConvertToEntity(conversationDto, advertisement, users, null);

            _dbContext.Conversation.Add(conversation);
            _dbContext.SaveChanges();
        }

        public void CreateMessage(MessageDto messageDto, int conversationId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversation = _dbContext.Conversation.Where(x => x.Id == conversationId).FirstOrDefault();

            if (conversation == null)
            {
                throw new ConversationNotFoundException("Conversation not exists.");
            }

            var userFrom = _dbContext.Users.Where(x => new Guid(x.Id) == messageDto.UserFrom.Id).FirstOrDefault();

            var message = _messageConverter.ConvertToEntity(messageDto, userFrom);

            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ConversationDto> GetAllConversation(string searchString)
        {
            var resultList = new List<ConversationDto>();
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = _dbContext.Conversation
                        .Where(c => c.Users.Any(u => u.Id == signedInUserId))
                        .Include(c => c.Advertisement)
                        .ToList();

            foreach (var con in conversations)
            {
                var dto = _conversationConverter.ConvertToDto(con);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<MessageDto> GetAllMessage(int conversationId)
        {

            var resultList = new List<MessageDto>();
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var msgs = _dbContext.Messages
                        .Where(m => m.Id == m.ConversationId)
                        .Include(m => m.UserFrom)
                        .ToList();

            foreach (var msg in msgs)
            {
                var dto = _messageConverter.ConvertToDto(msg, conversationId);
                resultList.Add(dto);
            }

            return resultList;
        }
    }
}
