using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class SportPriceModel
    {
        public SportPriceModel()
        {
            PriceList = new List<SportPriceList>();
        }

        [DisplayName("عدد الأشخاص")]
        [Required(ErrorMessage = "يرجى تعبئة الحقل")]
        public int Person { get; set; }

        public int SportId { get; set; }

        public List<SportPriceList> PriceList { get; set; }
    }
}
