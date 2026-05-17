using MazraeatiBackOffice.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MazraeatiBackOffice.Models
{
    public class AppUserBlackListModel
    {
        public int Id { get; set; }

        [DisplayName("رقم هاتف")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustMobileNum { get; set; }

        [DisplayName("الاسم بالعربى ان اوجد")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustName { get; set; }

        [DisplayName("الاسم بالانجليزى ان اوجد")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string CustNameEn { get; set; }

        [DisplayName("السبب بالعربى")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string Reason { get; set; }

        [DisplayName("السبب بالانجليزى")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string ReasonEn { get; set; }

        [DisplayName("رابط الصورة")]
        public string ImageUrl { get; set; }

        [DisplayName("هل  موافق عليها")]
        public bool IsApprove { get; set; }
        [DisplayName("المستخدم أو العميل")]
        public string UserDesc { get; set; }
        public int UserId { get; set; }
        [DisplayName("هل  موافق علي حظر هذه المستخدم أو العميل")]
        public bool IsBlocked { get; set; }

        public List<AppUser> Users { get; set; }
    }
}
