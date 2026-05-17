using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("TripVideo")]
    public class TripVideo : BaseEntity
    {
        public int TripId { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; }
    }
}
