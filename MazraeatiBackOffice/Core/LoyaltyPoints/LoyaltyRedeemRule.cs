using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// قواعد صرف النقاط (كم نقطة = كم دينار)
    /// </summary>
    [Table("LoyaltyRedeemRule")]
    public class LoyaltyRedeemRule : BaseEntity
    {
        [Required]
        public int Points { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
