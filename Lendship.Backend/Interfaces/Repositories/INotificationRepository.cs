using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetAll(string userId, string searchInAdvertisementTitle);

        IEnumerable<Notification> GetAllNew(string userId);

        void SetSeenNotifications(List<int> ids, string userId);

        void Create(Notification notification);
    }
}