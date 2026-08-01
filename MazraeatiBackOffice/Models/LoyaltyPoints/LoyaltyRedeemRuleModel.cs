using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class LoyaltyRedeemRuleModel
    {
        public int Id { get; set; }

        [Display(Name = "عدد النقاط")]
        [Required(ErrorMessage = "عدد النقاط مطلوب")]
        public int Points { get; set; }

        [Display(Name = "قيمة الخصم")]
        [Required(ErrorMessage = "قيمة الخصم مطلوبة")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
    }
}
