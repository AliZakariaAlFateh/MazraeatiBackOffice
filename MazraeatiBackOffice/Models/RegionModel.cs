using MazraeatiBackOffice.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MazraeatiBackOffice.Models
{
    public class RegionModel
    {
        public int Id { get; set; }

        [DisplayName("المدينة")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int CityId { get; set; }
        public string CityName { get; set; }

        [DisplayName("الوصف بالعربي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescAr { get; set; }

        [DisplayName("الوصف بالانجليزي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescEn { get; set; }

        public List<City> Cities { get; set; }
    }
}
