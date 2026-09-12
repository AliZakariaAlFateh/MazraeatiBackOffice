using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.SystemCore
{
    [Table("Region")]
    public class Regions: BaseEntity
    {
        public int CityId { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
    }
}
