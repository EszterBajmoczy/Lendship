using Lendship.Backend.Converters;
using Lendship.Backend.DTO;
using Lendship.Backend.Exceptions;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Interfaces.Services;
using Lendship.Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lendship.Backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly INotificationConverter _notificationConverter;

        public NotificationService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            //check at the end if the converters are needed!!!!!!!!!!!!!!!!!!!!!!!
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            _notificationConverter = new NotificationConverter();
        }

        public IEnumerable<NotificationDTO> GetAllNotifications()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.Notifications
                        .Where(n => n.UserId == signedInUserId)
                        .Select(n => _notificationConverter.ConvertToDto(n))
                        .ToList();
        }

        public IEnumerable<NotificationDTO> GetNewNotifications()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.Notifications
                        .Where(n => n.UserId == signedInUserId && n.New)
                        .Select(n => _notificationConverter.ConvertToDto(n))
                        .ToList();
        }

        public void SetSeenNotifications(List<int> ids)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = _dbContext.Notifications
                                    .Where(n => n.UserId == signedInUserId && n.New && ids.Contains(n.Id))
                                    .Select(n => _notificationConverter.ConvertToDto(n))
                                    .ToList();
            foreach (var noti in notifications)
            {
                noti.New = false;
            }

            _dbContext.UpdateRange(notifications);
            _dbContext.SaveChanges();
        }

        void INotificationService.CreateNotification(string msg, Reservation reservation, string userId)
        {
            var notification = new Notification()
            {
                UserId = userId,
                AdvertisementId = reservation.Advertisement.Id,
                AdvertisementTitle = reservation.Advertisement.Title,
                ReservationId = reservation.Id,
                UpdateInformation = msg,
                New = true
            };

            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();
        }
    }
}
