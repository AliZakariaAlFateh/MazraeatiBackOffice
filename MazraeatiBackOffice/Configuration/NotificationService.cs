using MazraeatiBackOffice.Core.FarmCore;
using MazraeatiBackOffice.Dto.NotificationDtos;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FirebaseNotificationService _notificationService;
        //private readonly IRepository<DeviceToken> _deviceToken;
        //private readonly IRepository<FarmerViewes> _farmerViewes;
        public NotificationService(
            INotificationRepository notificationRepository,
            IUnitOfWork unitOfWork, FirebaseNotificationService notificationService)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        // ==========================
        // قراءة الإشعارات غير المقروءة
        // ==========================
        public async Task<IEnumerable<NotificationDto>> GetUnreadNotifications()
        {
            return await _notificationRepository.GetUnreadNotifications();
        }

        // ==========================
        // قراءة إشعار معين
        // ==========================
        public async Task<NotificationDto> GetNotificationById(long id)
        {
            return await _notificationRepository.GetNotificationById(id);
        }

        // ==========================
        // عدد الإشعارات غير المقروءة
        // ==========================
        public async Task<int> GetUnreadCount()
        {
            return await _notificationRepository.GetUnreadCount();
        }


        //// ==========================
        //// تعيين إشعار كمقروء
        //// ==========================
        //public async Task<bool> MarkAsRead(long id)
        //{
        //    return await _notificationRepository.MarkAsRead(id);
        //}
        //// ==========================
        //// تعيين الكل كمقروء
        //// ==========================
        //public async Task<bool> MarkAllAsRead()
        //{
        //    return await _notificationRepository.MarkAllAsRead();
        //}
        //// ==========================
        //// تأكيد أو إلغاء الإشعار
        //// ==========================
        //public async Task<bool> ConfirmNotification(long id, bool isConfirmed)
        //{
        //    // ١- جلب الإشعار
        //    var notification = await _notificationRepository.GetNotificationById(id);
        //    if (notification == null)
        //        return false;

        //    // ٢- لو إشعار سعر وتم التأكيد → نحدث الأسعار
        //    if (notification.Type == "Price" && isConfirmed)
        //    {
        //        var newPrices = JsonConvert.DeserializeObject<List<FarmerPriceList>>(notification.NewData);

        //        if (newPrices != null && newPrices.Any())
        //        {
        //            foreach (var price in newPrices)
        //            {
        //                var existingPrice = _unitOfWork.FarmerPriceListRepository.GetById(price.Id);
        //                if (existingPrice != null)
        //                {
        //                    existingPrice.MorningPrice = price.MorningPrice;
        //                    existingPrice.EveningPrice = price.EveningPrice;
        //                    existingPrice.FullDayPrice = price.FullDayPrice;
        //                    existingPrice.OfferPrice = price.OfferPrice;
        //                    existingPrice.OfferEveningPrice = price.OfferEveningPrice;
        //                    existingPrice.OfferFullDayPrice = price.OfferFullDayPrice;
        //                    existingPrice.MorningPeriodText = price.MorningPeriodText;
        //                    existingPrice.EveningPeriodText = price.EveningPeriodText;
        //                    existingPrice.FullDayPeriodText = price.FullDayPeriodText;

        //                     _unitOfWork.FarmerPriceListRepository.Update(existingPrice);
        //                }
        //            }
        //             _unitOfWork.Save();
        //        }
        //    }

        //    // ٣- تحديث حالة الإشعار
        //    return await _notificationRepository.ConfirmNotification(id, isConfirmed);
        //}









        // Services/NotificationService.cs

        public async Task<bool> MarkAsRead(long id)
        {
            return await _notificationRepository.MarkAsRead(id);
        }

        public async Task<bool> MarkAllAsRead()
        {
            return await _notificationRepository.MarkAllAsRead();
        }

        public async Task<bool> ConfirmNotification(long id, bool isConfirmed)
        {
            var notification = await _notificationRepository.GetNotificationById(id);
            if (notification == null)
                return false;

            if (notification.Type == "Price" && isConfirmed)
            {
                var newPrices = JsonConvert.DeserializeObject<List<FarmerPriceList>>(notification.NewData);

                if (newPrices != null && newPrices.Any())
                {
                    int farmerId=0;
                    foreach (var price in newPrices)
                    {
                        var existingPrice =  _unitOfWork.FarmerPriceListRepository.GetById(price.Id);
                        
                        if (existingPrice != null)
                        {
                            farmerId = existingPrice.FarmerId;
                            existingPrice.MorningPrice = price.MorningPrice;
                            existingPrice.EveningPrice = price.EveningPrice;
                            existingPrice.FullDayPrice = price.FullDayPrice;
                            existingPrice.OfferPrice = price.OfferPrice;
                            existingPrice.OfferEveningPrice = price.OfferEveningPrice;
                            existingPrice.OfferFullDayPrice = price.OfferFullDayPrice;
                            existingPrice.MorningPeriodText = price.MorningPeriodText;
                            existingPrice.EveningPeriodText = price.EveningPeriodText;
                            existingPrice.FullDayPeriodText = price.FullDayPeriodText;

                             _unitOfWork.FarmerPriceListRepository.Update(existingPrice);
                        }
                    }
                     _unitOfWork.Save();
                    var farmerView = _unitOfWork.FarmerViewesRepository.Table.Where(fv => fv.FarmerId==farmerId).FirstOrDefault();
                    List<string> tokens = _unitOfWork.DeviceTokenRepository.Table
                                          .Where(c => c.DeviceId == farmerView.DeviceId)
                                          .Select(c => c.Token)
                                          .ToList(); // أو .ToList() إذا لم تكن تستخدم Async
                    var data = new Dictionary<string, string>
                    {
                        { "type", "ConfirmationPrice_notification" }
                    };
                    var farmName = _unitOfWork.FarmerRepository.Table
                                .FirstOrDefault(S => S.Id == farmerId).Name;

                    string title = " تأكيد تعديل الأسعار";
                    string source = "لوحة التحكم";

                    //string body = $"\u200Fتم إلغاء حجزك على النشاط الرياضي {sportName} بسبب: {reservation.Reason} (عبر {source})";
                    string body = string.Format("\u200Fتم تأكيد تعديل الإسعار لمزرعة  {0} (عبر {1})",
                                    farmName,          // {0}
                                    source             // {1}

                                );
                    if (tokens.Any())
                    {
                        await _notificationService.SendNotificationAsync(tokens, title, body, data);
                    }
                }
            }

            return await _notificationRepository.ConfirmNotification(id, isConfirmed);
        }









        // Services/NotificationService.cs
        public async Task<IEnumerable<NotificationDto>> GetAllNotifications(int page = 1, int pageSize = 20)
        {
            return await _notificationRepository.GetAllNotifications(page, pageSize);
        }

        public async Task<bool> CheckNotificationExists(string type, object dataId)
        {
            // ✅ التحقق من وجود إشعار بنفس النوع والبيانات
            var notifications = await _notificationRepository.GetAllNotifications();

            if (type == "Farm")
            {
                return notifications.Any(n =>
                    n.Type == "Farm" &&
                    !string.IsNullOrEmpty(n.NewData) &&
                    n.NewData.Contains(dataId.ToString()));
            }
            else if (type == "Price")
            {
                return notifications.Any(n =>
                    n.Type == "Price" &&
                    !string.IsNullOrEmpty(n.NewData) &&
                    n.NewData.Contains(dataId.ToString()));
            }

            return false;
        }

        //public async Task<int> DeleteOldNotifications(int days = 30)
        //{
        //    return await _notificationRepository.DeleteOldNotifications(days);
        //}

        //public async Task<int> GetUnreadCount()
        //{
        //    return await _notificationRepository.GetUnreadCount();
        //}
    }
}
