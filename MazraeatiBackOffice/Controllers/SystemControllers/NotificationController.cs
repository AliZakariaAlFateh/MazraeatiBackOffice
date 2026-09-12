using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core.SystemCore;
using MazraeatiBackOffice.Models.SystemModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers.SystemControllers
{
    //public class NotificationController : BaseController
    //{
    //    private readonly IRepository<DeviceToken> _deviceToken;
    //    private readonly FirebaseNotificationService _firebaseNotificationService;
    //    private readonly INotificationService _notificationService;

    //    public NotificationController(
    //        IRepository<DeviceToken> deviceToken,
    //        FirebaseNotificationService firebaseNotificationService,
    //        INotificationService notificationService)
    //    {
    //        _deviceToken = deviceToken;
    //        _firebaseNotificationService = firebaseNotificationService;
    //        _notificationService = notificationService;
    //    }



    //    // ==========================
    //    // عرض كل الإشعارات (بما فيها المقروءة)
    //    // ==========================
    //    public async Task<IActionResult> All(int page = 1, int pageSize = 20)
    //    {
    //        var notifications = await _notificationService.GetAllNotifications(page, pageSize);
    //        return View(notifications);
    //    }

    //    // ==========================
    //    // عرض صفحة الإشعارات
    //    // ==========================
    //    public async Task<IActionResult> Index()
    //    {
    //        var notifications = await _notificationService.GetUnreadNotifications();
    //        var count = await _notificationService.GetUnreadCount();
    //        ViewBag.UnreadCount = count;
    //        return View(notifications);
    //    }

    //    // ==========================
    //    // جلب الإشعارات (JSON) للـ JavaScript
    //    // ==========================
    //    //[HttpGet]
    //    //public async Task<IActionResult> GetNotifications()
    //    //{
    //    //    try
    //    //    {
    //    //        var notifications = await _notificationService.GetUnreadNotifications();
    //    //        var count = await _notificationService.GetUnreadCount();

    //    //        return Json(new
    //    //        {
    //    //            success = true,
    //    //            count = count,
    //    //            notifications = notifications
    //    //        });
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}








    //    // ==========================
    //    // تعيين إشعار كمقروء (زر تم المشاهدة)
    //    // ==========================
    //    //[HttpPost]
    //    //public async Task<IActionResult> MarkAsRead(long id)
    //    //{
    //    //    try
    //    //    {
    //    //        var result = await _notificationService.MarkAsRead(id);
    //    //        return Json(new { success = result });
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}

    //    //// ==========================
    //    //// تعيين الكل كمقروء
    //    //// ==========================
    //    //[HttpPost]
    //    //public async Task<IActionResult> MarkAllAsRead()
    //    //{
    //    //    try
    //    //    {
    //    //        var result = await _notificationService.MarkAllAsRead();
    //    //        return Json(new { success = result });
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}

    //    //// ==========================
    //    //// تأكيد الإشعار
    //    //// ==========================
    //    //[HttpPost]
    //    //public async Task<IActionResult> Confirm(long id, bool isConfirmed)
    //    //{
    //    //    try
    //    //    {
    //    //        var result = await _notificationService.ConfirmNotification(id, isConfirmed);

    //    //        if (result)
    //    //        {
    //    //            return Json(new
    //    //            {
    //    //                success = true,
    //    //                message = isConfirmed ? " تم تأكيد التحديث بنجاح" : "❌ تم إلغاء التحديث"
    //    //            });
    //    //        }
    //    //        else
    //    //        {
    //    //            return Json(new
    //    //            {
    //    //                success = false,
    //    //                message = " فشل تنفيذ العملية"
    //    //            });
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}

    //    //// ==========================
    //    //// عرض تفاصيل الإشعار
    //    //// ==========================
    //    //public async Task<IActionResult> Details(long id)
    //    //{
    //    //    var notification = await _notificationService.GetNotificationById(id);
    //    //    if (notification == null)
    //    //        return NotFound();
    //    //    return View(notification);
    //    //}





















    //    public IActionResult Send_Notification()
    //    {
    //        return View();
    //    }



    //    [HttpPost]
    //    public async Task<IActionResult> Send_Notification(SendNotificationModel model)
    //    {
    //        if (!ModelState.IsValid)
    //            return View(model);
    //        //&& x.Id== 17069
    //        var tokens = _deviceToken.Table
    //                                .Where(x => !string.IsNullOrEmpty(x.Token))
    //                                .Select(x => x.Token)
    //                                .Distinct()
    //                                .ToList();

    //        if (tokens == null || !tokens.Any())
    //        {
    //            TempData["Error"] = "لا يوجد أجهزة لإرسال الإشعار";
    //            return View(model);
    //        }
    //        var data = new Dictionary<string, string>
    //                {
    //                    { "type", "general_notification" }
    //                };
    //        await _firebaseNotificationService.SendNotificationAsync(tokens, model.Title, model.Body, data);

    //        TempData["Success"] = "تم إرسال الإشعار بنجاح ";

    //        return RedirectToAction("Send_Notification");
    //    }




    //    // Controllers/NotificationController.cs

    //    // ==========================
    //    // عرض تفاصيل الإشعار
    //    // ==========================
    //    public async Task<IActionResult> Details(long id)
    //    {
    //        try
    //        {
    //            var notification = await _notificationService.GetNotificationById(id);
    //            if (notification == null)
    //            {
    //                return NotFound();
    //            }

    //            // ✅ جلب البيانات من OldData و NewData
    //            object oldData = null;
    //            object newData = null;

    //            if (!string.IsNullOrEmpty(notification.OldData))
    //            {
    //                oldData = JsonConvert.DeserializeObject(notification.OldData);
    //            }

    //            if (!string.IsNullOrEmpty(notification.NewData))
    //            {
    //                newData = JsonConvert.DeserializeObject(notification.NewData);
    //            }

    //            ViewBag.OldData = oldData;
    //            ViewBag.NewData = newData;

    //            return View(notification);
    //        }
    //        catch (Exception ex)
    //        {
    //            ViewBag.Error = ex.Message;
    //            return View("Error");
    //        }
    //    }






    //    // Controllers/NotificationController.cs











    //    // ==========================
    //    // جلب الإشعارات من الـ Cache (بدون تحميل من قاعدة البيانات)
    //    // ==========================
    //    //[HttpGet]
    //    //public IActionResult GetNotifications()
    //    //{
    //    //    try
    //    //    {
    //    //        // ✅ جلب الإشعارات من الـ Cache (اللي بيحدثها الـ BackgroundService)
    //    //        var notifications = NotificationCache.Notifications;
    //    //        var count = NotificationCache.Count;

    //    //        return Json(new
    //    //        {
    //    //            success = true,
    //    //            count = count,
    //    //            notifications = notifications,
    //    //            lastUpdate = NotificationCache.LastUpdate
    //    //        });
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}





















    //    // ==========================
    //    // جلب عدد الإشعارات فقط (من الـ Cache)
    //    // ==========================
    //    //[HttpGet]
    //    //public IActionResult GetUnreadCount()
    //    //{
    //    //    try
    //    //    {
    //    //        var count = NotificationCache.Count;
    //    //        return Json(new { success = true, count = count });
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}

    //    // Controllers/NotificationController.cs

    //    // ==========================
    //    // جلب عدد الإشعارات غير المقروءة فقط
    //    // ==========================
    //    //[HttpGet]
    //    //public async Task<IActionResult> GetUnreadCount()
    //    //{
    //    //    try
    //    //    {
    //    //        var count = await _notificationService.GetUnreadCount();
    //    //        return Json(new { success = true, count = count });
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        return Json(new { success = false, message = ex.Message });
    //    //    }
    //    //}














    //    [HttpGet]
    //    public IActionResult GetNotifications()
    //    {
    //        try
    //        {
    //            var notifications = NotificationCache.Notifications;
    //            var count = NotificationCache.Count;

    //            return Json(new
    //            {
    //                success = true,
    //                count,
    //                notifications,
    //                lastUpdate = NotificationCache.LastUpdate
    //            });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = ex.Message });
    //        }
    //    }

    //    [HttpGet]
    //    public IActionResult GetUnreadCount()
    //    {
    //        try
    //        {
    //            var count = NotificationCache.Count;
    //            return Json(new { success = true, count });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = ex.Message });
    //        }
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> MarkAsRead(long id)
    //    {
    //        try
    //        {
    //            // ✅ تحديث في قاعدة البيانات
    //            var result = await _notificationService.MarkAsRead(id);

    //            if (result)
    //            {
    //                // ✅ تحديث في الكاش
    //                NotificationCache.MarkAsRead(id);

    //                return Json(new
    //                {
    //                    success = true,
    //                    message = "تم تعيين الإشعار كمقروء",
    //                    count = NotificationCache.Count
    //                });
    //            }

    //            return Json(new { success = false, message = "فشل التحديث" });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = ex.Message });
    //        }
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> MarkAllAsRead()
    //    {
    //        try
    //        {
    //            // ✅ تحديث في قاعدة البيانات
    //            var result = await _notificationService.MarkAllAsRead();

    //            if (result)
    //            {
    //                // ✅ تحديث في الكاش
    //                NotificationCache.MarkAllAsRead();

    //                return Json(new
    //                {
    //                    success = true,
    //                    message = "تم تعيين الكل كمقروء",
    //                    count = 0
    //                });
    //            }

    //            return Json(new { success = false, message = "فشل التحديث" });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = ex.Message });
    //        }
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Confirm(long id, bool isConfirmed)
    //    {
    //        try
    //        {
    //            // ✅ تحديث في قاعدة البيانات (مع التحديث الفعلي للأسعار)
    //            var result = await _notificationService.ConfirmNotification(id, isConfirmed);

    //            if (result)
    //            {
    //                // ✅ حذف من الكاش
    //                NotificationCache.ConfirmNotification(id, isConfirmed);

    //                return Json(new
    //                {
    //                    success = true,
    //                    message = isConfirmed ? "✅ تم تأكيد التحديث بنجاح" : "❌ تم إلغاء التحديث",
    //                    count = NotificationCache.Count
    //                });
    //            }

    //            return Json(new { success = false, message = "فشل تنفيذ العملية" });
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new { success = false, message = ex.Message });
    //        }
    //    }











    //    }


    public class NotificationController : BaseController
    {
        private readonly IRepository<DeviceToken> _deviceToken;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly INotificationService _notificationService;

        public NotificationController(
            IRepository<DeviceToken> deviceToken,
            FirebaseNotificationService firebaseNotificationService,
            INotificationService notificationService)
        {
            _deviceToken = deviceToken;
            _firebaseNotificationService = firebaseNotificationService;
            _notificationService = notificationService;
        }

        // ==========================
        // عرض صفحة الإشعارات
        // ==========================
        public async Task<IActionResult> Index()
        {
            var notifications = await _notificationService.GetUnreadNotifications();
            var count = await _notificationService.GetUnreadCount();
            ViewBag.UnreadCount = count;
            return View(notifications);
        }

        // ==========================
        // جلب الإشعارات (JSON) - من Database مباشرة
        // ==========================
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetUnreadNotifications();
                var count = await _notificationService.GetUnreadCount();

                return Json(new
                {
                    success = true,
                    count = count,
                    notifications = notifications,
                    lastUpdate = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ==========================
        // جلب عدد الإشعارات - من Database مباشرة
        // ==========================
        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var count = await _notificationService.GetUnreadCount();
                return Json(new { success = true, count = count });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ==========================
        // تعيين إشعار كمقروء
        // ==========================
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            try
            {
                var result = await _notificationService.MarkAsRead(id);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "تم تعيين الإشعار كمقروء"
                    });
                }

                return Json(new { success = false, message = "فشل التحديث" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ==========================
        // تعيين الكل كمقروء
        // ==========================
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                var result = await _notificationService.MarkAllAsRead();

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = "تم تعيين الكل كمقروء"
                    });
                }

                return Json(new { success = false, message = "فشل التحديث" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ==========================
        // تأكيد الإشعار
        // ==========================
        [HttpPost]
        public async Task<IActionResult> Confirm(long id, bool isConfirmed)
        {
            try
            {
                var result = await _notificationService.ConfirmNotification(id, isConfirmed);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = isConfirmed ? "✅ تم تأكيد التحديث بنجاح" : "❌ تم إلغاء التحديث"
                    });
                }

                return Json(new { success = false, message = "فشل تنفيذ العملية" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ==========================
        // عرض تفاصيل الإشعار
        // ==========================
        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var notification = await _notificationService.GetNotificationById(id);
                if (notification == null)
                {
                    return NotFound();
                }

                object oldData = null;
                object newData = null;

                if (!string.IsNullOrEmpty(notification.OldData))
                {
                    oldData = JsonConvert.DeserializeObject(notification.OldData);
                }

                if (!string.IsNullOrEmpty(notification.NewData))
                {
                    newData = JsonConvert.DeserializeObject(notification.NewData);
                }

                ViewBag.OldData = oldData;
                ViewBag.NewData = newData;

                return View(notification);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        // ==========================
        // عرض كل الإشعارات
        // ==========================
        public async Task<IActionResult> All(int page = 1, int pageSize = 20)
        {
            var notifications = await _notificationService.GetAllNotifications(page, pageSize);
            return View(notifications);
        }

        // ==========================
        // إرسال إشعار
        // ==========================
        public IActionResult Send_Notification()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send_Notification(SendNotificationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var tokens = _deviceToken.Table
                                    .Where(x => !string.IsNullOrEmpty(x.Token))
                                    .Select(x => x.Token)
                                    .Distinct()
                                    .ToList();

            if (tokens == null || !tokens.Any())
            {
                TempData["Error"] = "لا يوجد أجهزة لإرسال الإشعار";
                return View(model);
            }

            var data = new Dictionary<string, string>
            {
                { "type", "general_notification" }
            };

            await _firebaseNotificationService.SendNotificationAsync(tokens, model.Title, model.Body, data);

            TempData["Success"] = "تم إرسال الإشعار بنجاح";

            return RedirectToAction("Send_Notification");
        }
    }
}
