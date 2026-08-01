using System;

namespace MazraeatiBackOffice.Core
{
    public class UserLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string ScreenUrl { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public virtual AdminUser User { get; set; }
    }
}
