using MazraeatiBackOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class CityModel
    {
        public int Id { get; set; }

        [DisplayName("الدولة")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        [DisplayName("الوصف بالعربي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescAr { get; set; }

        [DisplayName("الوصف بالانجليزي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescEn { get; set; }

        [DisplayName("هل فعال ؟")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public bool Active { get; set; }
        public List<Country> Countries { get; set; }
    }
}
