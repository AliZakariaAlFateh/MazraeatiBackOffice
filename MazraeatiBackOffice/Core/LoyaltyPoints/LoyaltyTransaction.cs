using MazraeatiBackOffice.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// حركات النقاط (سجل كل العمليات)
    /// </summary>
    [Table("LoyaltyTransaction")]
    public class LoyaltyTransaction : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }  // long بدل int
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public TransactionTypeEnum TransactionType { get; set; }
        [Required]
        public int Points { get; set; }
        public int? ReferenceId { get; set; }
        [MaxLength(50)]
        public string ReferenceType { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public DateTime? ExpireDate { get; set; }
        public int? CreatedBy { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual AdminUser CreatedByUser { get; set; }
        //Why Use AppUser Here 
        //is this for Dashboard to check who make adjust on the transactions
    }
}
