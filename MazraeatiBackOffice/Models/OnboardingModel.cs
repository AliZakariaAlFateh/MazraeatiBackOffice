using System;
using MazraeatiBackOffice.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class OnboardingModel
    {
        public int Id { get; set; }

        [DisplayName("الدولة")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int CountryId { get; set; }
        public string CountryDesc { get; set; }
        public int Type { get; set; }

        [DisplayName("رابط الصورة")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string Url { get; set; }

        [DisplayName("نص اضافي فوق الصورة")]
        public string ExtraText { get; set; }

        [DisplayName("تاريخ انتهاء الاعلان")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ExpiryDate { get; set; }
        public List<Country> Countries { get; set; }
    }
}
