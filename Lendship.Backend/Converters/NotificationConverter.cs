using Lendship.Backend.DTO;
using Lendship.Backend.Interfaces.Converters;
using Lendship.Backend.Models;

namespace Lendship.Backend.Converters
{
    public class NotificationConverter : INotificationConverter
    {
        public NotificationDTO ConvertToDto(Notification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                UserId = notification.UserId,
                AdvertisementId = notification.AdvertisementId,
                AdvertisementTitle = notification.AdvertisementTitle,
                ReservationId = notification.ReservationId,
                UpdateInformation = notification.UpdateInformation,
                New = notification.New
            };
        }
    }
}
