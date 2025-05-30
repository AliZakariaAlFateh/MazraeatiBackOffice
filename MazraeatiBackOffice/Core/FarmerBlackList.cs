using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerBlackList")]
    public class FarmerBlackList : BaseEntity
    {
        public string FarmerName { get; set; }
        public string FarmerMobNum { get; set; }
        public string Reason { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApprove { get; set; }
        public int? FarmerId { get; set; }
    }
}
