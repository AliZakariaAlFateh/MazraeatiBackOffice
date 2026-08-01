using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class AdditionalServiceModel
    {
        public int Id { get; set; }

        [DisplayName("الخدمة بالعربي")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        [MaxLength(200)]
        public string ServiceTextAr { get; set; }

        [DisplayName("الخدمة بالإنجليزي")]
        [MaxLength(200)]
        public string ServiceTextEn { get; set; }

        [DisplayName("الأيقونة")]
        [MaxLength(50)]
        public string IconClass { get; set; }

        [DisplayName("نشط")]
        public bool IsActive { get; set; } = true;
    }
}
