using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.LoyaltyPoints
{
    /// <summary>
    /// أنواع الأنشطة (مزرعة، كرة قدم، سباحة، إلخ)
    /// </summary>
    //[Table("LoyaltyActivityType")]
    //public class LoyaltyActivityType : BaseEntity
    //{
    //    [Required(ErrorMessage = "الاسم بالعربي مطلوب")]
    //    [MaxLength(100)]
    //    public string NameAr { get; set; }

    //    [MaxLength(100)]
    //    public string NameEn { get; set; }

    //    [Required(ErrorMessage = "الكود مطلوب")]
    //    [MaxLength(50)]
    //    public string Code { get; set; }

    //    public int? SportTypeId { get; set; }

    //    [MaxLength(50)]
    //    public string IconClass { get; set; }

    //    public bool IsActive { get; set; } = true;

    //    public DateTime CreatedDate { get; set; } = DateTime.Now;
    //    public DateTime? ModifiedDate { get; set; }
    //    // ===== Navigation Properties =====
    //    [ForeignKey("SportTypeId")]
    //    public virtual SportType SportType { get; set; }

    //    public virtual ICollection<LoyaltyPointRuleSport> PointRulesSport { get; set; }
    //    public virtual ICollection<LoyaltyPointRuleFarm> PointRulesFarm { get; set; }
    //    public virtual ICollection<LoyaltyPointRuleGeneral> PointRulesGeneral { get; set; }
    //    public virtual ICollection<LoyaltyBookingActivity> BookingActivities { get; set; }
    //}

    [Table("LoyaltyActivityType")]
    public class LoyaltyActivityType : BaseEntity
    {
        [Required(ErrorMessage = "الاسم بالعربي مطلوب")]
        [MaxLength(100)]
        public string NameAr { get; set; }

        [MaxLength(100)]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "الكود مطلوب")]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string ReferenceTable { get; set; }  // ✅ 'Farm', 'Sports', 'Restaurants'

        public int? SportTypeId { get; set; }

        [MaxLength(50)]
        public string IconClass { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("SportTypeId")]
        public virtual SportType SportType { get; set; }

        public virtual ICollection<LoyaltyPointRule> PointRules { get; set; }
        public virtual ICollection<LoyaltyBookingActivity> BookingActivities { get; set; }
    }


}
