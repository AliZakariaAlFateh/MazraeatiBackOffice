using MazraeatiBackOffice.Configuration;
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
        public int CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string CustMobNum { get; set; }
        public string CustomerName { get; set; }
        public ReservStatusEnum ReservStatus { get; set; } = ReservStatusEnum.Pending;
        public string Reason { get; set; }

        #region new attributes
        public int NumberOfPerson { get; set; }
        public decimal CostReservationAmtOnMahjouz { get; set; }
        public decimal ReservationAmt { get; set; }
        public decimal NetProfit { get; set; }
        public decimal ReservationDepositAmt { get; set; }
        public decimal ReservationRemainAmt { get; set; }

        #endregion
        public string Note { get; set; }
        public bool IsReciveCommission { get; set; }
        public string AutomaticallyNote { get; set; }
        public string MobileOwnerAppUser { get; set; }
        public bool? IsMahjouzReservation { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Farmer Farm { get; set; }
    }
}
