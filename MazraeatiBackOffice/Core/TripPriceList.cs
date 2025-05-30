using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("TripPriceList")]
    public class TripPriceList : BaseEntity
    {
        public int TripId { get; set; }
        public int? Person { get; set; }
        public decimal Price { get; set; }
        public decimal OfferPrice { get; set; }
    }
}
