using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class SportReservationModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "برجاء اختيار القسم الرياضي")]
        public int SportTypeId { get; set; }
        [Required(ErrorMessage = "برجاء اختيار العقار")]
        public int SportId { get; set; }
        //[Required(ErrorMessage = "برجاء اختيار العميل")]
        public int CustomerId { get; set; }
        [DisplayName("القسم الرياضة")]
        public string SportTypeDesc { get; set; }

        [DisplayName("العقار")]
        public int SportDesc { get; set; }

        [DisplayName("العميل")]
        public int CustomerDesc { get; set; }

        public string SportName { get; set; }

        [DisplayName("اسم العميل")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string CustomerName { get; set; }

        [DisplayName("رقم جوال العميل")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string CustMobNum { get; set; }

        [DisplayName("رقم جوال المالك")]
        public string MobileOwnerAppUser { get; set; }

        [DisplayName("تاريخ الحجز")]
        [Required(ErrorMessage = "برجاء اختيار التاريخ")]
        public DateTime ReservationDate { get; set; }

        [DisplayName("وقت البداية")]
        [Required(ErrorMessage = "برجاء اختيار وقت البداية")]
        public TimeSpan StartTime { get; set; }

        [DisplayName("وقت النهاية")]
        [Required(ErrorMessage = "برجاء اختيار وقت النهاية")]
        public TimeSpan EndTime { get; set; }

        [DisplayName("عدد الساعات")]
        public int TotalHours { get; set; }

        [DisplayName("عدد الأشخاص")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int PersonCount { get; set; }

        [DisplayName("تكلفة الحجز على محجوز")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public decimal CostReservationAmtOnMahjouz { get; set; }

        [DisplayName("المبلغ الكلي المطلوب")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public decimal ReservationAmt { get; set; }

        [DisplayName("المبلغ المدفوع")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public decimal ReservationDepositAmt { get; set; }

        [DisplayName("الربح")]
        public decimal NetProfit { get; set; }

        [DisplayName("المبلغ المتبقي")]
        public decimal ReservationRemainAmt { get; set; }

        [DisplayName("حالة الحجز")]
        public ReservStatusEnum ReservStatus { get; set; } = ReservStatusEnum.Pending;

        [DisplayName("سبب الإلغاء")]
        public string Reason { get; set; }

        [DisplayName("ملاحظات")]
        public string Note { get; set; }

        [DisplayName("حجز من الداش بورد")]
        public bool IsMahjouzReservation { get; set; } = true;
        [DisplayName("هل استلمت العمولة")]
        public bool IsReceiveCommession { get; set; } = false;
        [DisplayName("Device ID")]
        public string DeviceId { get; set; }

        [DisplayName("Token العميل")]
        public string TokenCustomer { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        // Lists for dropdowns
        public List<SportType>? SportTypes { get; set; } = new List<SportType>();
        public List<Sport>? Sports { get; set; } = new List<Sport>();
        public List<Customer>? Customers { get; set; } = new List<Customer>();

        // ===== نظام النقاط =====
        [DisplayName("استخدام نقاط الولاء")]
        public bool UseLoyaltyPoints { get; set; } = false;

        [DisplayName("عدد النقاط للخصم")]
        public int RedeemPoints { get; set; } = 0;

        [DisplayName("قيمة الخصم")]
        public decimal DiscountAmount { get; set; } = 0;

        [DisplayName("نقاط العميل المتاحة")]
        public int CustomerAvailablePoints { get; set; } = 0;

    }
}
