using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class LoyaltyPointRuleModel
    {
        public int Id { get; set; }

        [Display(Name = "نوع النشاط")]
        [Required(ErrorMessage = "نوع النشاط مطلوب")]
        public int ActivityTypeId { get; set; }

        [Display(Name = "نوع المرجع")]
        [Required(ErrorMessage = "نوع المرجع مطلوب")]
        public string ReferenceType { get; set; }

        [Display(Name = "المرجع")]
        public int? ReferenceId { get; set; }

        [Display(Name = "الكود")]
        public string Code { get; set; }

        [Display(Name = "اسم المرجع")]
        public string ReferenceName { get; set; }

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
