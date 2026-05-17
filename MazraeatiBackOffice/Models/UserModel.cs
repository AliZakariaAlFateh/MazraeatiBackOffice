using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MazraeatiBackOffice.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        //[DisplayName("المزرعة")]
        //[Required(ErrorMessage = "يرجى تعبئه الحقل")]
        //public int FarmId { get; set; }
        //public string FarmName { get; set; }

        [DisplayName("المزارع")]
        [Required(ErrorMessage = "يرجى اختيار مزرعة واحدة على الأقل")]
        public List<int> FarmIds { get; set; } = new List<int>();  // ✅ Multi Select
        public List<SelectListItem> Farms { get; set; } = new List<SelectListItem>(); // للعرض

        [DisplayName("اسم المستخدم")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string UserName { get; set; }

        [DisplayName("رقم الواتس آب")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string MobilePhone { get; set; }

        [DisplayName("رقم التليفون")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string MobileNumber { get; set; }

        [DisplayName("كلمة المرور")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string PasswordHash { get; set; }
        //public List<Farmer> Farmers { get; set; }
    }
}
