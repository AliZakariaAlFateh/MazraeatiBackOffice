using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Dto
{
    public class SportPriceDayDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int Day { get; set; }

        [DisplayName("سعر الساعة العادية")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public decimal HourlyPrice { get; set; }

        [DisplayName("سعر ساعة الذروة")]
        public decimal? PeakHourlyPrice { get; set; }

        [DisplayName("بداية وقت الذروة")]
        public TimeSpan? PeakStartTime { get; set; }

        [DisplayName("نهاية وقت الذروة")]
        public TimeSpan? PeakEndTime { get; set; }

        [DisplayName("سعر العرض")]
        public decimal? OfferHourlyPrice { get; set; }
    }
}
