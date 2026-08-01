using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    [Table("LoyaltyPointRule")]
    public class LoyaltyPointRule : BaseEntity
    {
        [Required]
        public int ActivityTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ReferenceType { get; set; }  // 'Farm', 'Sport'
        public int? ReferenceId { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        [Required]
        public int Points { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        [ForeignKey("ActivityTypeId")]
        public virtual LoyaltyActivityType ActivityType { get; set; }
    }
}
