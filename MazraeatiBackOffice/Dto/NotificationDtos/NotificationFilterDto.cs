using System;

namespace MazraeatiBackOffice.Dto.NotificationDtos
{
    public class NotificationFilterDto
    {
        public string Type { get; set; } // 'Farm', 'Price', null = الكل
        public bool? IsRead { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool? IsCancelled { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
