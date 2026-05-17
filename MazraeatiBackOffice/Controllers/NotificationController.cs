using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IRepository<DeviceToken> _deviceToken;
        private readonly FirebaseNotificationService _notificationService;

        public NotificationController(
            IRepository<DeviceToken> deviceToken,
            FirebaseNotificationService notificationService)
        {
            _deviceToken = deviceToken;
            _notificationService = notificationService;
        }
        [HttpGet("/notifications")]
        public IActionResult Index()
        {
            var messages = SignalRListenerFarms.GetNotifications();
            return Json(messages);
        }
        public IActionResult Send_Notification()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Send_Notification(SendNotificationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            //&& x.Id== 17069
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
            await _notificationService.SendNotificationAsync(tokens, model.Title, model.Body, data);

            TempData["Success"] = "تم إرسال الإشعار بنجاح ✅";

            return RedirectToAction("Send_Notification");
        }

    }
}
