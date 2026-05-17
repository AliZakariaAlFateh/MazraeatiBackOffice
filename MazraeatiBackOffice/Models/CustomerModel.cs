using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MazraeatiBackOffice.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }

        [DisplayName("اسم المستخدم")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string FullName { get; set; }

        [DisplayName("رقم الواتس آب")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string MobileNumber { get; set; }


    }
}
