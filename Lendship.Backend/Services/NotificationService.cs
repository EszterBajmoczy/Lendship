using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationConverter _notificationConverter;

        public NotificationService(
            IHttpContextAccessor httpContextAccessor, 
            INotificationRepository notificationRepository,
            INotificationConverter notificationConverter)
        {
            _httpContextAccessor = httpContextAccessor;
            _notificationRepository = notificationRepository;

            _notificationConverter = notificationConverter;
        }

        public IEnumerable<NotificationDTO> GetAllNotifications(string searchInAdvertisementTitle)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _notificationRepository.GetAll(signedInUserId, searchInAdvertisementTitle)
                        .Select(n => _notificationConverter.ConvertToDto(n));
        }

        public int GetNewNotificationCount()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _notificationRepository.GetAllNew(signedInUserId)
                        .Select(n => _notificationConverter.ConvertToDto(n))
                        .Count();
        }

        public void SetSeenNotifications(List<int> ids)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _notificationRepository.SetSeenNotifications(ids, signedInUserId);
        }

        void INotificationService.CreateNotification(string msg, Reservation reservation, string userId)
        {
            var notification = new Notification()
            {
                UserId = userId,
                AdvertisementId = reservation.Advertisement.Id,
                AdvertisementTitle = reservation.Advertisement.Title,
                ReservationDateFrom = reservation.DateFrom,
                ReservationDateTo = reservation.DateTo,
                ReservationId = reservation.Id,
                UpdateInformation = msg,
                New = true,
                TimeSpan = DateTime.Now
            };

            _notificationRepository.Create(notification);
        }
    }
}
