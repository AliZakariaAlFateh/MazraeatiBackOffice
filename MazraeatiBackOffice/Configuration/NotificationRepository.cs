using FirebaseAdmin.Messaging;
using MazraeatiBackOffice.Core.SystemCore;
using MazraeatiBackOffice.Dto.NotificationDtos;
using MazraeatiBackOffice.Extenstion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly DataContext _context;
        private readonly DbSet<Notifications> _dbSet;

        public NotificationRepository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<Notifications>();
        }

        // ==========================
        // قراءة الإشعارات غير المقروءة
        // ==========================
        public async Task<IEnumerable<NotificationDto>> GetUnreadNotifications()
        {
            var entities = await _dbSet
                .Where(n => n.IsRead == false)  // && !n.IsDeleted
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return entities.Select(e => e.ToDto());
        }

        // ==========================
        // قراءة إشعار معين
        // ==========================
        public async Task<NotificationDto> GetNotificationById(long id)
        {
            var entity = await _dbSet
                .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted);

            return entity?.ToDto();
        }

        // ==========================
        // عدد الإشعارات غير المقروءة
        // ==========================
        public async Task<int> GetUnreadCount()
        {
            return await _dbSet
                .CountAsync(n => !n.IsRead); //&& !n.IsDeleted
        }

        // ==========================
        // تعيين إشعار كمقروء
        // ==========================
        public async Task<bool> MarkAsRead(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null || entity.IsDeleted)
                return false;

            entity.MarkAsRead();
            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================
        // تعيين الكل كمقروء
        // ==========================
        public async Task<bool> MarkAllAsRead()
        {
            var entities = await _dbSet
                .Where(n => !n.IsRead && !n.IsDeleted)
                .ToListAsync();

            foreach (var entity in entities)
            {
                entity.MarkAsRead();
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================
        // تأكيد أو إلغاء الإشعار
        // ==========================
        public async Task<bool> ConfirmNotification(long id, bool isConfirmed)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null || entity.IsDeleted)
                return false;

            entity.Confirm(isConfirmed);
            await _context.SaveChangesAsync();
            return true;
        }


        // Repositories/NotificationRepository.cs
        public async Task<IEnumerable<NotificationDto>> GetAllNotifications(int page = 1, int pageSize = 20)
        {
            var entities = await _dbSet
                .Where(n => !n.IsDeleted)
                .OrderByDescending(n => n.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return entities.Select(e => e.ToDto());
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotifications()
        {
            var entities = await _dbSet
                .Where(n => !n.IsDeleted)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();

            return entities.Select(e => e.ToDto());
        }


        //public async Task<int> DeleteReadNotifications()
        //{
        //    var entities = await _dbSet
        //        .Where(n => n.IsRead && !n.IsDeleted)
        //        .ToListAsync();

        //    foreach (var entity in entities)
        //    {
        //        entity.UpdateForDelete();
        //    }

        //    await _context.SaveChangesAsync();
        //    return entities.Count;
        //}

        //public async Task<int> DeleteOldNotifications(int days = 30)
        //{
        //    var cutoffDate = DateTime.Now.AddDays(-days);
        //    var entities = await _dbSet
        //        .Where(n => n.CreatedDate < cutoffDate && !n.IsDeleted)
        //        .ToListAsync();

        //    foreach (var entity in entities)
        //    {
        //        entity.UpdateForDelete();
        //    }

        //    await _context.SaveChangesAsync();
        //    return entities.Count;
        //}

        //public async Task<int> GetUnreadCount()
        //{
        //    return await _dbSet
        //        .CountAsync(n => !n.IsRead && !n.IsDeleted);
        //}
    }
}
