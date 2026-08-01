using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportFeatures")]
    public class SportFeature:BaseEntity
    {
        public int SportTypeId { get; set; }
        public string FeatureTextAr { get; set; }
        public string FeatureTextEn { get; set; }
        public string IconClass { get; set; }
        public bool IsActive { get; internal set; }
    }
}
