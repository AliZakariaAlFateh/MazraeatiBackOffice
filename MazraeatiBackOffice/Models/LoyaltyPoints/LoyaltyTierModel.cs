using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class LoyaltyTierModel
    {
        public int Id { get; set; }

        [Display(Name = "الاسم بالعربي")]
        [Required(ErrorMessage = "الاسم بالعربي مطلوب")]
        public string NameAr { get; set; }

        [Display(Name = "الاسم بالإنجليزي")]
        public string NameEn { get; set; }

        [Display(Name = "الأيقونة")]
        public string IconClass { get; set; }

        [Display(Name = "الحد الأدنى للنقاط")]
        [Required(ErrorMessage = "الحد الأدنى مطلوب")]
        public int MinPoints { get; set; }

        [Display(Name = "الحد الأقصى للنقاط")]
        public int? MaxPoints { get; set; }

        [Display(Name = "نسبة الخصم")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
    }
}
