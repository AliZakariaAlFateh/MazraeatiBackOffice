using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class LoyaltyActivityTypeModel
    {
        public int Id { get; set; }

        [Display(Name = "الاسم بالعربي")]
        [Required(ErrorMessage = "الاسم بالعربي مطلوب")]
        public string NameAr { get; set; }

        [Display(Name = "الاسم بالإنجليزي")]
        public string NameEn { get; set; }

        [Display(Name = "الكود")]
        [Required(ErrorMessage = "الكود مطلوب")]
        public string Code { get; set; }
        [MaxLength(50)]
        public string ReferenceTable { get; set; }  // 'Farm', 'Sports', 'Restaurants'
        [Display(Name = "نوع الرياضة")]
        public int? SportTypeId { get; set; }

        [Display(Name = "الأيقونة")]
        public string IconClass { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
