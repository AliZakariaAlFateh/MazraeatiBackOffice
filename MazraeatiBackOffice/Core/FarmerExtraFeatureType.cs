using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerExtraFeatureType")]
    public class FarmerExtraFeatureType : BaseEntity
    {
        public int FarmerId { get; set; }
        public int TypeId { get; set; }
        public string ExtraText { get; set; }
        public string ExtraTextDescriptionAr { get; set; }
        public string ExtraTextDescriptionEn { get; set; }
     

    }
}
