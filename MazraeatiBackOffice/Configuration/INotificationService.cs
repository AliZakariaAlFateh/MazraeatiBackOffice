using MazraeatiBackOffice.Dto.NotificationDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public interface INotificationService
    {
        // قراءة الإشعارات
        Task<IEnumerable<NotificationDto>> GetUnreadNotifications();
        Task<NotificationDto> GetNotificationById(long id);
        Task<int> GetUnreadCount();

        // تحديث الإشعارات
        Task<bool> MarkAsRead(long id);
        Task<bool> MarkAllAsRead();
        Task<bool> ConfirmNotification(long id, bool isConfirmed);
        Task<IEnumerable<NotificationDto>> GetAllNotifications(int page = 1, int pageSize = 20);
        Task<bool> CheckNotificationExists(string type, object dataId);
        //// إشعارات المزرعة (عرض فقط)
        //Task NotifyFarmAdded(object farmData);

        //// إشعارات الأسعار (بحاجة تأكيد)
        //Task NotifyPriceUpdated(object oldPrices, object newPrices);
    }
}
