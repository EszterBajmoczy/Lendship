﻿using Lendship.Backend.Converters;
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

namespace Lendship.Backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LendshipDbContext _dbContext;
        private readonly INotificationConverter _notificationConverter;

        public NotificationService(IHttpContextAccessor httpContextAccessor, LendshipDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            //TODO inject converters!!
            _notificationConverter = new NotificationConverter();
        }

        public IEnumerable<NotificationDTO> GetAllNotifications(string searchInAdvertisementTitle)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.Notifications
                        .Where(n => n.UserId == signedInUserId && (searchInAdvertisementTitle == null || n.AdvertisementTitle.Contains(searchInAdvertisementTitle)))
                        .OrderByDescending(n => n.TimeSpan)
                        .Select(n => _notificationConverter.ConvertToDto(n))
                        .ToList();
        }

        public IEnumerable<NotificationDTO> GetNewNotifications()
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.Notifications
                        .Where(n => n.UserId == signedInUserId && n.New)
                        .OrderByDescending(n => n.TimeSpan)
                        .Select(n => _notificationConverter.ConvertToDto(n))
                        .ToList();
        }

        public void SetSeenNotifications(List<int> ids)
        {
            var signedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = _dbContext.Notifications
                                    .Where(n => n.UserId == signedInUserId && n.New && ids.Contains(n.Id));
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
                ReservationDateFrom = reservation.DateFrom,
                ReservationDateTo = reservation.DateTo,
                ReservationId = reservation.Id,
                UpdateInformation = msg,
                New = true,
                TimeSpan = DateTime.Now
            };

            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();
        }
    }
}
