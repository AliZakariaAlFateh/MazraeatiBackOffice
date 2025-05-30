using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    public class FarmerExtraFeatureTypeDto 
    {
        public int Id { get; set; }
        public int FarmerId { get; set; }
        public int TypeId { get; set; }
        public string DescAr { get; set; }
        public string ExtraText { get; set; }
        public string ExtraTextDescription { get; set; }
        public bool IsCheck { get; set; }
    }
}
