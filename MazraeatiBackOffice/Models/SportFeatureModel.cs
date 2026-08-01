using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class SportFeatureModel
    {
        public int Id { get; set; }

        public int SportTypeId { get; set; }

        [DisplayName("نوع القسم الرياضي")]
        public string SportTypeName { get; set; }

        [DisplayName("المرفق بالعربي")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        [MaxLength(200)]
        public string FeatureTextAr { get; set; }

        [DisplayName("المرفق بالإنجليزي")]
        [MaxLength(200)]
        public string FeatureTextEn { get; set; }

        [DisplayName("الأيقونة")]
        [MaxLength(50)]
        public string IconClass { get; set; }

        [DisplayName("نشط")]
        public bool IsActive { get; set; } = true;
        //Note the CreatedDate and ModifiedDate are Important in any table ....
        //public DateTime CreatedDate { get; set; }
        //public DateTime? ModifiedDate { get; set; }

    }
}
