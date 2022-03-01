using Lendship.Backend.DTO;
using Lendship.Backend.Models;
using System.Collections.Generic;

namespace Lendship.Backend.Interfaces.Services
{
    public interface INotificationService
    {
        void CreateNotification(string msg, Reservation reservation, string UserId);

        IEnumerable<NotificationDTO> GetAllNotifications();

        IEnumerable<NotificationDTO> GetNewNotifications();

        void SetSeenNotifications(List<int> ids);
    }
}
