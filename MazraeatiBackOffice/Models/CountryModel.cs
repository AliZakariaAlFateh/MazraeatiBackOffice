using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class CountryModel
    {
        public int Id { get; set; }

        [DisplayName("رمز الدولة")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string Code { get; set; }

        [DisplayName("الوصف بالعربي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescAr { get; set; }

        [DisplayName("الوصف بالانجليزي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescEn { get; set; }

        [DisplayName("رابط الصورة")]
        public string ImageUrl { get; set; }

        [DisplayName("هل فعال ؟")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public bool Active { get; set; }
    }
}
