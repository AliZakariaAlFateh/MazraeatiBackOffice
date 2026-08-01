using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class SportTypeModel
    {
        public int Id { get; set; }

        [DisplayName("اسم النوع بالعربي")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        [MaxLength(50)]
        public string NameAr { get; set; }

        [DisplayName("اسم النوع بالإنجليزي")]
        [MaxLength(50)]
        public string NameEn { get; set; }

        [DisplayName("الأيقونة")]
        [MaxLength(50)]
        public string IconClass { get; set; }

        [DisplayName("نشط")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
