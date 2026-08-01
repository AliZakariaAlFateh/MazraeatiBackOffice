using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// قواعد النقاط للأنشطة العامة (مطاعم، فنادق، إلخ)
    /// </summary>
    [Table("LoyaltyPointRuleGeneral")]
    public class LoyaltyPointRuleGeneral : BaseEntity
    {
        [Required]
        public int ActivityTypeId { get; set; }

        [Required]
        public int Points { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        // ===== Navigation Properties =====
        [ForeignKey("ActivityTypeId")]
        public virtual LoyaltyActivityType ActivityType { get; set; }
    }
}
