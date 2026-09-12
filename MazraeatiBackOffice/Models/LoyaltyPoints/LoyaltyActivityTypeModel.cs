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
        public string ReferenceTable { get; set; }  // 'Farmer', 'Sports', 'Restaurants'
        //ال id الخاص بجدول نوع العقار فى أي نشاط
        //سواء كان نشاط رياضي أو أكواخ أو ترفيه 
        //لكن الحاجات اللى ما لها أنواع فى الانشطه مثل المزارع 
        //بيكون ال id=null or 0  تمام 

        public int? CategoryId { get; set; }
        //[Display(Name = "نوع الرياضة")]
        //public int? SportTypeId { get; set; }

        [Display(Name = "الأيقونة")]
        public string IconClass { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
