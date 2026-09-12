using System;

namespace MazraeatiBackOffice.Dto.NotificationDtos
{
    public class NotificationDto
    {
        public long Id { get; set; } //long
        public string Type { get; set; } // 'Farm', 'Price'
        public string Title { get; set; }
        public string Message { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
