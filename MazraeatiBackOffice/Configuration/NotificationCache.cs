using MazraeatiBackOffice.Dto.NotificationDtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MazraeatiBackOffice.Configuration
{
    public static class NotificationCache
    {
        //public static List<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();
        //public static int Count { get; set; } = 0;
        //public static DateTime LastUpdate { get; set; } = DateTime.MinValue;




        private static readonly ConcurrentDictionary<long, NotificationDto> _notifications = new ConcurrentDictionary<long, NotificationDto>();
        private static int _count = 0;
        private static DateTime _lastUpdate = DateTime.MinValue;
        private static readonly object _lock = new object();

        public static List<NotificationDto> Notifications
        {
            get
            {
                lock (_lock)
                {
                    return _notifications.Values.OrderByDescending(n => n.CreatedDate).ToList();
                }
            }
        }

        public static int Count
        {
            get
            {
                lock (_lock)
                {
                    return _count;
                }
            }
            private set
            {
                lock (_lock)
                {
                    _count = value;
                }
            }
        }

        public static DateTime LastUpdate
        {
            get
            {
                lock (_lock)
                {
                    return _lastUpdate;
                }
            }
            private set
            {
                lock (_lock)
                {
                    _lastUpdate = value;
                }
            }
        }

        // ✅ تحديث الكاش بالكامل
        public static void UpdateCache(IEnumerable<NotificationDto> notifications)
        {
            lock (_lock)
            {
                _notifications.Clear();

                if (notifications != null)
                {
                    foreach (var notification in notifications)
                    {
                        _notifications.TryAdd(notification.Id, notification);
                    }

                    Count = notifications.Count();
                }
                else
                {
                    Count = 0;
                }

                LastUpdate = DateTime.Now;
            }
        }

        // ✅ إضافة إشعار جديد
        public static void AddNotification(NotificationDto notification)
        {
            lock (_lock)
            {
                if (!_notifications.ContainsKey(notification.Id))
                {
                    _notifications.TryAdd(notification.Id, notification);
                    Count = _notifications.Count;
                    LastUpdate = DateTime.Now;
                }
            }
        }

        // ✅ تعيين إشعار كمقروء
        public static bool MarkAsRead(long id)
        {
            lock (_lock)
            {
                if (_notifications.TryGetValue(id, out var notification))
                {
                    notification.IsRead = true;
                    notification.ReadDate = DateTime.Now;
                    _notifications[id] = notification;

                    // ✅ تحديث العدد
                    Count = _notifications.Values.Count(n => !n.IsRead && !n.IsDeleted);
                    LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        // ✅ تأكيد أو إلغاء إشعار (مع الحذف من الكاش)
        public static bool ConfirmNotification(long id, bool isConfirmed)
        {
            lock (_lock)
            {
                if (_notifications.TryRemove(id, out _))
                {
                    // ✅ حذف الإشعار من الكاش
                    Count = _notifications.Values.Count(n => !n.IsRead && !n.IsDeleted);
                    LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        // ✅ تعيين الكل كمقروء
        public static void MarkAllAsRead()
        {
            lock (_lock)
            {
                foreach (var key in _notifications.Keys.ToList())
                {
                    if (_notifications.TryGetValue(key, out var notification))
                    {
                        notification.IsRead = true;
                        notification.ReadDate = DateTime.Now;
                        _notifications[key] = notification;
                    }
                }

                Count = 0;
                LastUpdate = DateTime.Now;
            }
        }

        // ✅ حذف إشعار
        public static bool DeleteNotification(long id)
        {
            lock (_lock)
            {
                if (_notifications.TryRemove(id, out _))
                {
                    Count = _notifications.Values.Count(n => !n.IsRead && !n.IsDeleted);
                    LastUpdate = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        // ✅ إعادة تحميل الكاش من قاعدة البيانات
        public static void RefreshCache(IEnumerable<NotificationDto> notifications)
        {
            UpdateCache(notifications);
        }






    }
}
