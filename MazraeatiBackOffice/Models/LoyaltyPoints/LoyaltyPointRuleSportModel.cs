using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    //public class LoyaltyPointRuleSportModel
    //{
    //    public int Id { get; set; }

    //    [Display(Name = "نوع النشاط")]
    //    [Required(ErrorMessage = "نوع النشاط مطلوب")]
    //    public int ActivityTypeId { get; set; }

    //    [Display(Name = "العقار الرياضي")]
    //    public int? SportId { get; set; }

    //    [Display(Name = "عدد النقاط")]
    //    [Required(ErrorMessage = "عدد النقاط مطلوب")]
    //    public int Points { get; set; }

    //    [Display(Name = "نشط")]
    //    public bool IsActive { get; set; } = true;

    //    [Display(Name = "قاعدة افتراضية")]
    //    public bool IsDefault { get; set; } = false;

    //    public DateTime CreatedDate { get; set; }
    //    public DateTime? ModifiedDate { get; set; }
    //}
    public class LoyaltyPointRuleSportModel
    {
        public int Id { get; set; }

        [Display(Name = "نوع النشاط")]
        [Required(ErrorMessage = "نوع النشاط مطلوب")]
        public int ActivityTypeId { get; set; }

        [Display(Name = "العقار الرياضي")]
        public int? SportId { get; set; }

        [Display(Name = "المزرعة")]
        public int? FarmId { get; set; }  // ✅ جديد للمزارع

        [Display(Name = "عدد النقاط")]
        [Required(ErrorMessage = "عدد النقاط مطلوب")]
        public int Points { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "قاعدة افتراضية")]
        public bool IsDefault { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
