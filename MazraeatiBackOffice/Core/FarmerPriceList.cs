using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerPriceList")]
    public class FarmerPriceList : BaseEntity
    {
        public int FarmerId { get; set; }
        public int Day { get; set; }
        public int? Person { get; set; }
        public decimal? MorningPrice { get; set; }
        public decimal? EveningPrice { get; set; }
        public decimal? FullDayPrice { get; set; }
        public decimal? OfferPrice { get; set; }
        public decimal? OfferEveningPrice { get; set; }
        public decimal? OfferFullDayPrice { get; set; }
        public string? MorningPeriodText { get; set; }
        public string? EveningPeriodText { get; set; }
        public string? FullDayPeriodText { get; set; }
    }
}
