using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportSafetyFeatures")]
    public class SportSafetyFeature:BaseEntity
    {
        public int SportId { get; set; }

        public int SafetyFeatureId { get; set; }

        public bool IsChecked { get; set; } = false;
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }


    }
}
