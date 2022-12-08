using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConversationRepository _conversationRepository;
        private readonly IUsersAndConversationsRepository _usersAndConversationsRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        private readonly IConversationConverter _conversationConverter;
        private readonly IMessageConverter _messageConverter;

        public ConversationService(
            IHttpContextAccessor httpContextAccessor, 
            IConversationRepository conversationRepository,
            IUsersAndConversationsRepository usersAndConversationsRepository,
            IAdvertisementRepository advertisementRepository,
            IUserRepository userRepository,
            IMessageRepository messageRepository,
            IConversationConverter conversationConverter,
            IMessageConverter messageConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _conversationRepository = conversationRepository;
            _usersAndConversationsRepository = usersAndConversationsRepository;
            _advertisementRepository = advertisementRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;

            _conversationConverter = conversationConverter;
            _messageConverter = messageConverter;
        }

        public int CreateConversation(ConversationDto conversationDto)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var advertisement = _advertisementRepository.GetPlainById(conversationDto.AdvertisementId, signedInUserId);

            if (advertisement == null)
            {
                throw new AdvertisementNotFoundException("Advertisement not exists.");
            }

            var conversation = _conversationConverter.ConvertToEntity(conversationDto, advertisement, null);

            _conversationRepository.Create(conversation);
            _usersAndConversationsRepository.Create(conversation.Id, signedInUserId, advertisement.User.Id);

            return conversation.Id;
        }

        public void CreateMessage(MessageDto messageDto)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversation = _conversationRepository.GetById(messageDto.ConversationId);

            if (conversation == null)
            {
                throw new ConversationNotFoundException("Conversation not exists.");
            }

            var userFrom = _userRepository.GetById(signedInUserId);

            var message = _messageConverter.ConvertToEntity(messageDto, userFrom);
            _messageRepository.Create(message);
        }

        public IEnumerable<ConversationDto> GetAllConversation(string searchString)
        {
            var resultList = new List<ConversationDto>();
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var conversations = _usersAndConversationsRepository.Get(signedInUserId)
                            .Select(x => x.Conversation)
                            .ToList();
            
            foreach (var con in conversations)
            {
                var users = _usersAndConversationsRepository.GetById(con.Id, signedInUserId)
                            .Select(x => x.User)
                            .ToList();

                var hasNewMessage = _messageRepository.HasNewMessage(con.Id, signedInUserId);

                var dto = _conversationConverter.ConvertToDto(con, users, hasNewMessage);
                resultList.Add(dto);
            }

            return resultList;
        }

        public IEnumerable<MessageDto> GetAllMessage(int conversationId)
        {
            var resultList = _messageRepository.GetByConversation(conversationId)
                .Select(x => _messageConverter.ConvertToDto(x, conversationId));

            return resultList;
        }

        public int GetNewMessageCount()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _usersAndConversationsRepository.GetNewMessagesCount(signedInUserId);
        }

        public void SetMessagesSeen(int conversationId)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var connection = _usersAndConversationsRepository.GetById(conversationId, signedInUserId);

            if (connection != null)
            {
                _messageRepository.SetMessagesSeen(conversationId, signedInUserId);
            }
        }
    }
}
