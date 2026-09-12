using MazraeatiBackOffice.Dto.NotificationDtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class NotificationBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger;
        private System.Timers.Timer _timer;

        public NotificationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 Notification Background Service Started");

            // ✅ تشغيل Timer كل 30 ثانية
            _timer = new System.Timers.Timer();
            _timer.Interval = 3000;
            _timer.Elapsed += async (sender, e) => await LoadNotificationsFromDatabase();
            _timer.AutoReset = true;
            _timer.Enabled = true;

            // ✅ أول تحميل فوراً
            Task.Run(async () => await LoadNotificationsFromDatabase());

            return Task.CompletedTask;
        }

        private async Task LoadNotificationsFromDatabase()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    // ✅ جلب الإشعارات غير المقروءة من قاعدة البيانات
                    var notifications = await notificationService.GetUnreadNotifications();

                    // ✅ تحديث الكاش
                    NotificationCache.UpdateCache(notifications);

                    _logger.LogInformation($"📦 Cache updated: {NotificationCache.Count} notifications");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error loading notifications: {ex.Message}");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🛑 Notification Background Service Stopped");
            _timer?.Stop();
            _timer?.Dispose();
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }



        //private readonly IServiceProvider _serviceProvider;
        //private readonly ILogger<NotificationBackgroundService> _logger;
        //private System.Timers.Timer _timer;

        //public NotificationBackgroundService(
        //    IServiceProvider serviceProvider,
        //    ILogger<NotificationBackgroundService> logger)
        //{
        //    _serviceProvider = serviceProvider;
        //    _logger = logger;
        //}

        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("🚀 Notification Background Service Started");

        //    // ✅ تشغيل Timer كل 30 ثانية
        //    _timer = new System.Timers.Timer();
        //    _timer.Interval = 30000; // 30 ثانية
        //    _timer.Elapsed += async (sender, e) => await LoadNotifications();
        //    _timer.AutoReset = true;
        //    _timer.Enabled = true;

        //    return Task.CompletedTask;
        //}

        //private async Task LoadNotifications()
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        //            // ✅ جلب الإشعارات غير المقروءة من قاعدة البيانات
        //            var notifications = await notificationService.GetUnreadNotifications();
        //            var count = await notificationService.GetUnreadCount();

        //            if (notifications.Any())
        //            {
        //                _logger.LogInformation($"📦 {count} unread notifications loaded from database");

        //                // ✅ تخزين الإشعارات في Cache أو Static Variable
        //                NotificationCache.Notifications = notifications.ToList();
        //                NotificationCache.Count = count;
        //                NotificationCache.LastUpdate = DateTime.Now;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"❌ Error loading notifications: {ex.Message}");
        //    }
        //}

        //public override async Task StopAsync(CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation("🛑 Notification Background Service Stopped");
        //    _timer?.Stop();
        //    _timer?.Dispose();
        //    await base.StopAsync(cancellationToken);
        //}

        //public override void Dispose()
        //{
        //    _timer?.Dispose();
        //    base.Dispose();
        //}




    }
}
