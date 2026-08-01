using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerReservationModel
    {
        public int Id { get; set; }

        [DisplayName("المزرعة")]
        //[Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public int FarmerId { get; set; }
        public int CustomerId { get; set; }
        [DisplayName("نوع الحجز")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int ReservationTypeId { get; set; }
        public string ReservationTypeDesc { get; set; }
        [DisplayName("العميل")]
        public int CustomerDesc { get; set; }
        [DisplayName("تاريخ الحجز")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public DateTime ReservationDate { get; set; }

        [DisplayName("رقم هاتف العميل")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustMobNum { get; set; }
        public string Reason { get; set; }
        [DisplayName("اسم العميل")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustomerName { get; set; }
        public ReservStatusEnum ReservStatus { get; set; } = ReservStatusEnum.Pending;

        [DisplayName("ملاحظات")]
        public string Note { get; set; }

        #region ِadded attributes ...
        [DisplayName("عدد الأشخاص")]
        public int NumberOfPerson { get; set; }
        [DisplayName("تكلفة حجز المزرعة على محجوز")]
        public decimal CostReservationAmtOnMahjouz { get; set; }
        [DisplayName("المبلغ الكلى المطلوب للحجز")]
        public decimal ReservationAmt { get; set; }
        [DisplayName("صافى الربح")]
        public decimal NetProfit { get; set; }
        [DisplayName("المبلغ المدفوع")]
        public decimal ReservationDepositAmt { get; set; }
        [DisplayName("المبلغ المتبقى")]
        public decimal ReservationRemainAmt { get; set; }
        #endregion

        [DisplayName("هل استلمت العمولة")]
        public bool IsReciveCommission { get; set; }
        public DateTime CreatedDate { get; set; }
        [DisplayName("ملاحظات من قبل محجوز لا يمكن التعديل عليها")]
        public string AutomaticallyNote { get; set; }
        public string MobileOwnerAppUser { get; set; }
        public bool? IsMahjouzReservation { get; set; }
        public List<LookupValue> LookupValues { get; set; } = new List<LookupValue>();
        public List<Farmer> Farms { get; set; } = new List<Farmer>();
        public List<Customer>? Customers { get; set; } = new List<Customer>();
    }
}
