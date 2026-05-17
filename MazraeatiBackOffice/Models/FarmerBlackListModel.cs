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

        [DisplayName(" الاسم ان اوجد بالعربي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string FarmerName { get; set; }
        [DisplayName(" الاسم ان اوجد بالانجليزي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string FarmerNameEn { get; set; }

        [DisplayName("السبب بالعربي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string Reason { get; set; }
        [DisplayName("السبب بالانجليزي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string ReasonEn { get; set; }

        [DisplayName("رابط الصورة")]
        public string ImageUrl { get; set; }

        [DisplayName("هل  موافق عليها")]
        public bool IsApprove { get; set; }
        public int? FarmerId { get; set; }
        [DisplayName("هل  موافق علي حظر هذه المزرعه")]
        public bool IsBlocked { get; set; }

        public List<Farmer> Farmers { get; set; }
    }
}
