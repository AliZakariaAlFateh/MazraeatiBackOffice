using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MazraeatiBackOffice.Models.UserManagementModel
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

        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }

    }
}
