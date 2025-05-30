using MazraeatiBackOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerBlackListModel
    {
        public int Id { get; set; }

        [DisplayName("رقم هاتف")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string FarmerMobNum { get; set; }

        [DisplayName("الاسم ان اوجد")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string FarmerName { get; set; }

        [DisplayName("السبب")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string Reason { get; set; }

        [DisplayName("رابط الصورة")]
        public string ImageUrl { get; set; }

        [DisplayName("هل  موافق عليها")]
        public bool IsApprove { get; set; } 
    }
}
