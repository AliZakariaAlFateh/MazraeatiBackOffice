using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// ربط الحجوزات بالأنشطة
    /// </summary>
    [Table("LoyaltyBookingActivity")]
    public class LoyaltyBookingActivity : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string BookingType { get; set; }

        [Required]
        public int ActivityTypeId { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("ActivityTypeId")]
        public virtual LoyaltyActivityType ActivityType { get; set; }
    }
}
