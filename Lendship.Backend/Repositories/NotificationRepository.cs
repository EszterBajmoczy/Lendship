using Lendship.Backend.Interfaces.Repositories;
using Lendship.Backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lendship.Backend.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly LendshipDbContext _dbContext;

        public NotificationRepository(LendshipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Notification> GetAll(string userId, string searchInAdvertisementTitle)
        {
            return _dbContext.Notifications
                        .Where(n => n.UserId == userId
                            && (searchInAdvertisementTitle == null || n.AdvertisementTitle.Contains(searchInAdvertisementTitle)))
                        .OrderByDescending(n => n.TimeSpan);
        }

        public IEnumerable<Notification> GetAllNew(string userId)
        {
            return _dbContext.Notifications
                        .Where(n => n.UserId == userId && n.New)
                        .OrderByDescending(n => n.TimeSpan);
        }

        public void SetSeenNotifications(List<int> ids, string userId)
        {
            var notifications = _dbContext.Notifications
                                    .Where(n => n.UserId == userId && n.New && ids.Contains(n.Id));
            foreach (var noti in notifications)
            {
                noti.New = false;
            }

            _dbContext.UpdateRange(notifications);
            _dbContext.SaveChanges();
        }
    }
}
