using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SafetyFeatures")]
    public class SafetyFeature:BaseEntity
    {
        public string FeatureTextAr { get; set; }

        public string FeatureTextEn { get; set; }

        public string IconClass { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}
