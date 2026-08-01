using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// خصومات النقاط على الحجوزات
    /// </summary>
    [Table("ReservationLoyaltyDiscount")]
    public class ReservationLoyaltyDiscount : BaseEntity
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ReservationType { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int PointsUsed { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
