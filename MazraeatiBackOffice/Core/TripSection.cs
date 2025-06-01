using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core
{
    [Table("TripSection")]
    public class TripSection : BaseEntity
    {
        public int CountryId { get; set; }
        public string Title { get; set; }
        public string MainImage { get; set; }
        public string ExtraText { get; set; }
        public string ExtraTextEn { get; set; }

        public bool Active { get; set; }
        public int OrderId { get; set; }
    }
}
