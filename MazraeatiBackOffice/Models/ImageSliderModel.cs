using System;
using MazraeatiBackOffice.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class ImageSliderModel
    {
        public int Id { get; set; }

        [DisplayName("الدولة")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int CountryId { get; set; }
        public string CountryDesc { get; set; }

        [DisplayName("موقع البانر")]
        public string PageName { get; set; }

        [DisplayName("رابط الصورة")]
        public string Image { get; set; }

        [DisplayName("نص اضافي فوق الصورة بالعربى")]
        public string ExtraText { get; set; }

        [DisplayName("نص اضافي فوق الصورة بالانجليزى")]
        public string ExtraTextEn { get; set; }

        [DisplayName("رابط خارجي ان اوجد")]
        public string RedirectLink { get; set; }

        [DisplayName("الهدف")]
        public string Target { get; set; }

        [DisplayName("قيمة الاستهداف")]
        public string Value { get; set; }

        [DisplayName("ترتيب العرض")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int SortOrder { get; set; }

        [DisplayName("هل فعال ؟")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public bool Active { get; set; }
        public List<Country> Countries { get; set; }
    }
}
