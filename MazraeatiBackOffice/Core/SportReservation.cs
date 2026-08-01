using MazraeatiBackOffice.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportReservations")]
    public class SportReservation : BaseEntity
    {
        public int? SportId { get; set; }
        public int? SportTypeId { get; set; }
        public int? CustomerId { get; set; }
        // بيانات العميل
        public string CustomerName { get; set; }
        public string CustMobNum { get; set; }

        // بيانات المالك (تؤخذ من Sport)
        public string MobileOwnerAppUser { get; set; }

        // تفاصيل الحجز
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalHours { get; set; }
        public int PersonCount { get; set; }

        // الأسعار
        public decimal CostReservationAmtOnMahjouz { get; set; }
        public decimal ReservationAmt { get; set; }
        public decimal ReservationDepositAmt { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ReservationRemainAmt { get; set; }

        // حالة الحجز
        public ReservStatusEnum ReservStatus { get; set; } = ReservStatusEnum.Pending;
        public string Reason { get; set; }

        // معلومات إضافية
        public string Note { get; set; }

        public bool IsMahjouzReservation { get; set; } = true;
        public bool IsReceiveCommession { get; set; } = false;
        public string DeviceId { get; set; }
        public string TokenCustomer { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        //for Include Methods ....
        // Navigation Properties
        //[ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        //[ForeignKey("SportId")]
        public virtual Sport Sport { get; set; }

        //[ForeignKey("SportTypeId")]
        public virtual SportType SportType { get; set; }
    }
}
