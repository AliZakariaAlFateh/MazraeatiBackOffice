using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// مستويات العملاء (برونز، فضي، ذهبي، ألماس)
    /// </summary>
    [Table("LoyaltyTier")]
    public class LoyaltyTier : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string NameAr { get; set; }
        [MaxLength(50)]
        public string NameEn { get; set; }
        [MaxLength(50)]
        public string IconClass { get; set; }
        [Required]
        public int MinPoints { get; set; }
        public int? MaxPoints { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercent { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual ICollection<CustomerLoyaltyAccount> CustomerAccounts { get; set; }
    }
}
