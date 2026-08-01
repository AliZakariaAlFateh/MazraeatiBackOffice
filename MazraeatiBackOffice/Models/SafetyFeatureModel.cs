using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class SafetyFeatureModel
    {
        public int Id { get; set; }

        [DisplayName("الميزة بالعربي")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        [MaxLength(200)]
        public string FeatureTextAr { get; set; }

        [DisplayName("الميزة بالإنجليزي")]
        [MaxLength(200)]
        public string FeatureTextEn { get; set; }

        [DisplayName("الأيقونة")]
        [MaxLength(50)]
        public string IconClass { get; set; }

        [DisplayName("نشط")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
