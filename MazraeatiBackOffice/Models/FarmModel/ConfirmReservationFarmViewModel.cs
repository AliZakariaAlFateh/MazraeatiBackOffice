using System;

namespace MazraeatiBackOffice.Models.FarmModel
{
    public class ConfirmReservationFarmViewModel
    {
        //public int ReservationId { get; set; }
        //public int CustomerId { get; set; }
        //public string CustomerName { get; set; }
        //public string CustomerPhone { get; set; }
        //public string SportTypeName { get; set; }
        //public string FarmName { get; set; }
        //public DateTime ReservationDate { get; set; }
        //public decimal OriginalAmount { get; set; }
        //public decimal NetProfit { get; set; }

        //// ===== نظام النقاط =====
        //public int CustomerAvailablePoints { get; set; }
        //public string CurrentTierName { get; set; }
        //public string TierIcon { get; set; }
        //public string TierColor { get; set; }
        //public int EarnedPoints { get; set; }
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string FarmName { get; set; }
        public string ReservationTypeName { get; set; }
        public DateTime ReservationDate { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal NetProfit { get; set; }
        public int CustomerAvailablePoints { get; set; }
        public string CurrentTierName { get; set; }
        public int EarnedPoints { get; set; }
        public string TierIcon { get; set; }
    }
}
