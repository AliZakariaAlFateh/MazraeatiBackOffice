using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// حساب الولاء للعميل
    /// </summary>
    [Table("CustomerLoyaltyAccount")]
    public class CustomerLoyaltyAccount : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }
        public int TotalPoints { get; set; } = 0;
        public int AvailablePoints { get; set; } = 0;
        public int RedeemedPoints { get; set; } = 0;
        public int ExpiredPoints { get; set; } = 0;
        public int? CurrentTierId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("CurrentTierId")]
        public virtual LoyaltyTier CurrentTier { get; set; }
    }
}
