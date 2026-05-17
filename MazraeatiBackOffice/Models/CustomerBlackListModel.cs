using MazraeatiBackOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class CustomerBlackListModel
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

    }
}
