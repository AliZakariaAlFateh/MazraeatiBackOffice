using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("TripExtraFeatureType")]
    public class TripExtraFeatureType : BaseEntity
    {
        public int TripId { get; set; }
        public int TypeId { get; set; }
        public string ExtraText { get; set; }
    }
}
