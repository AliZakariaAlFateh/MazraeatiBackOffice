using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class ReservationLoyaltyDiscountModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم الحجز")]
        [Required(ErrorMessage = "رقم الحجز مطلوب")]
        public int ReservationId { get; set; }

        [Display(Name = "نوع الحجز")]
        [Required(ErrorMessage = "نوع الحجز مطلوب")]
        public string ReservationType { get; set; }

        [Display(Name = "العميل")]
        [Required(ErrorMessage = "العميل مطلوب")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        [Display(Name = "النقاط المستخدمة")]
        [Required(ErrorMessage = "النقاط المستخدمة مطلوبة")]
        public int PointsUsed { get; set; }

        [Display(Name = "قيمة الخصم")]
        [Required(ErrorMessage = "قيمة الخصم مطلوبة")]
        public decimal DiscountAmount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
