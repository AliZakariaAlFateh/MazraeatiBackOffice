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
        public int FarmerId { get; set; }

        [DisplayName("نوع الحجز")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int ReservationTypeId { get; set; }
        public string ReservationTypeDesc { get; set; }

        [DisplayName("تاريخ الحجز")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public DateTime ReservationDate { get; set; }

        [DisplayName("رقم هاتف العميل")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustMobNum { get; set; }

        [DisplayName("اسم العميل")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustomerName { get; set; }

        [DisplayName("ملاحظات")]
        public string Note { get; set; }

        [DisplayName("هل استلمت العمولة")]
        public bool IsReciveCommission { get; set; }
        public DateTime CreatedDate { get; set; }
        [DisplayName("ملاحظات من قبل محجوز لا يمكن التعديل عليها")]
        public string AutomaticallyNote { get; set; }

        public List<LookupValue> LookupValues { get; set; }
    }
}
