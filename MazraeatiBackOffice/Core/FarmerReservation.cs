using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerReservation")]
    public class FarmerReservation : BaseEntity
    {
        public int FarmerId { get; set; }
        public int ReservationTypeId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string CustMobNum { get; set; }
        public string CustomerName { get; set; }
        public string Note { get; set; }
        public bool IsReciveCommission { get; set; }
        public string AutomaticallyNote { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
