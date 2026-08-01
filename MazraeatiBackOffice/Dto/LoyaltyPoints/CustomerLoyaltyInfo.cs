namespace MazraeatiBackOffice.Dto.LoyaltyPoints
{
    public class CustomerLoyaltyInfo
    {
        public int CustomerId { get; set; }
        public int AvailablePoints { get; set; }
        public int TotalPoints { get; set; }
        public string TierName { get; set; }
        public string TierIcon { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
