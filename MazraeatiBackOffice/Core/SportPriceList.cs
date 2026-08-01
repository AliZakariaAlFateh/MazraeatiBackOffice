using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportPriceList")]
    public class SportPriceList:BaseEntity
    {
        public int SportId { get; set; }
        public int Day { get; set; } // 1=الأحد, 2=الاثنين, ..., 7=السبت
        public int Person { get; set; } // عدد الأشخاص
        public decimal HourlyPrice { get; set; } // سعر الساعة العادية
        public decimal? PeakHourlyPrice { get; set; } // سعر ساعة الذروة
        public TimeSpan? PeakStartTime { get; set; } // بداية وقت الذروة
        public TimeSpan? PeakEndTime { get; set; } // نهاية وقت الذروة
        public decimal? OfferHourlyPrice { get; set; } // سعر العرض (الساعة)
        public int MinBookingHours { get; set; } = 1; // الحد الأدنى لساعات الحجز
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
