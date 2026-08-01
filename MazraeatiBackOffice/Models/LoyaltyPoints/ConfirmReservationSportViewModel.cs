using System;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class ConfirmReservationSportViewModel
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string SportTypeName { get; set; }
        public string SportName { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalHours { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal NetProfit { get; set; }

        // ===== نظام النقاط =====
        public int CustomerAvailablePoints { get; set; }
        public string CurrentTierName { get; set; }
        public string TierIcon { get; set; }
        public string TierColor { get; set; }

        public int EarnedPoints { get; set; }
    }
}
