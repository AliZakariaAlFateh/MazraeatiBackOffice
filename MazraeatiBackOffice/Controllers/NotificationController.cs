using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Mvc;

namespace MazraeatiBackOffice.Controllers
{
    public class NotificationController : Controller
    {
        [HttpGet("/notifications")]
        public IActionResult Index()
        {
            var messages = SignalRListenerFarms.GetNotifications();
            return Json(messages);
        }
    }
}
