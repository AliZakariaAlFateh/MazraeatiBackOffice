using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class GeneralFacilityModel
    {
        public int Id { get; set; }

        [DisplayName("المرفق بالعربي")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        [MaxLength(200)]
        public string FacilityTextAr { get; set; }

        [DisplayName("المرفق بالإنجليزي")]
        [MaxLength(200)]
        public string FacilityTextEn { get; set; }

        [DisplayName("الأيقونة")]
        [MaxLength(50)]
        public string IconClass { get; set; }

        [DisplayName("نشط")]
        public bool IsActive { get; set; } = true;
    }
}
