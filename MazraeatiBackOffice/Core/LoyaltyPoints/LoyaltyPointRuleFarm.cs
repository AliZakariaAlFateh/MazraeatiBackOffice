using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// قواعد النقاط للمزارع (خاصة وعامة)
    /// </summary>
    [Table("LoyaltyPointRuleFarm")]
    public class LoyaltyPointRuleFarm : BaseEntity
    {
        [Required]
        public int ActivityTypeId { get; set; }

        public int? FarmId { get; set; }

        [Required]
        public int Points { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        // ===== Navigation Properties =====
        [ForeignKey("ActivityTypeId")]
        public virtual LoyaltyActivityType ActivityType { get; set; }

        [ForeignKey("FarmId")]
        public virtual Farmer Farm { get; set; }
    }
}
