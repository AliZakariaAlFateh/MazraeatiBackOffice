using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Core.CottageCore;
using MazraeatiBackOffice.Core.SportCore;
using MazraeatiBackOffice.Dto;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.CottageModel
{
    public class CottagePriceModel
    {
        public CottagePriceModel()
        {
            PriceList = new List<CottagePriceList>();
        }

        [DisplayName("عدد الأشخاص")]
        [Required(ErrorMessage = "يرجى تعبئة الحقل")]
        public int Person { get; set; }

        public int CottageId { get; set; }

        public List<CottagePriceList> PriceList { get; set; }
    }
}
