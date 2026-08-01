using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class CustomerLoyaltyAccountModel
    {
        public int Id { get; set; }

        [Display(Name = "العميل")]
        [Required(ErrorMessage = "العميل مطلوب")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        [Display(Name = "إجمالي النقاط")]
        public int TotalPoints { get; set; }

        [Display(Name = "النقاط المتاحة")]
        public int AvailablePoints { get; set; }

        [Display(Name = "النقاط المستخدمة")]
        public int RedeemedPoints { get; set; }

        [Display(Name = "النقاط المنتهية")]
        public int ExpiredPoints { get; set; }

        [Display(Name = "المستوى الحالي")]
        public int? CurrentTierId { get; set; }
        public string CurrentTierName { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? ExpireDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
