using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class UserModel
    {


        //[DisplayName("المزرعة")]
        //[Required(ErrorMessage = "يرجى تعبئه الحقل")]
        //public int FarmId { get; set; }
        //public string FarmName { get; set; }


        ////Old
        //public int Id { get; set; }
        //[DisplayName("المزارع")]
        //[Required(ErrorMessage = "إضافة مزرعة")]
        //public List<int> FarmIds { get; set; } = new List<int>();  // ✅ Multi Select
        //public List<SelectListItem> Farms { get; set; } = new List<SelectListItem>(); // للعرض

        //[DisplayName("العقار الرياضي")]
        //[Required(ErrorMessage = "إضافة عقار رياضي")]
        //public List<int> SportIds { get; set; } = new List<int>();  // ✅ Multi Select
        //public List<SelectListItem> Sports { get; set; } = new List<SelectListItem>(); // للعرض

        //[DisplayName("اسم المستخدم")]
        //[Required(ErrorMessage = "يرجى تعبئه الحقل")]
        //public string UserName { get; set; }

        //[DisplayName("رقم الواتس آب")]
        //[Required(ErrorMessage = "يرجى تعبئه الحقل")]
        //public string MobilePhone { get; set; }

        //[DisplayName("رقم التليفون")]
        //[Required(ErrorMessage = "يرجى تعبئه الحقل")]
        //public string MobileNumber { get; set; }

        //[DisplayName("كلمة المرور")]
        //[Required(ErrorMessage = "يرجى تعبئه الحقل")]
        //public string PasswordHash { get; set; }
        //[DisplayName("يملك عقار")]
        //public UserTypeEnum UserType { get; set; }
        //[DisplayName("تفعيل العميل")]
        //public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }

        //New
        public int Id { get; set; }

        [DisplayName("المزارع")]
        public List<int> FarmIds { get; set; } = new List<int>();
        public List<SelectListItem> Farms { get; set; } = new List<SelectListItem>();

        [DisplayName("العقار الرياضي")]
        public List<int> SportIds { get; set; } = new List<int>();
        public List<SelectListItem> Sports { get; set; } = new List<SelectListItem>();

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

        // ===== نوع العقار (string للتخزين، List للـ MultiSelect) =====
        [DisplayName("نوع العقار")]
        public string UserType { get; set; } = "0";

        public List<string> UserTypeListSelected { get; set; } = new List<string>();
        public List<SelectListItem> UserTypeList { get; set; } = new List<SelectListItem>();

        [DisplayName("تفعيل العميل")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
