using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportGeneralFacilities")]
    public class SportGeneralFacility:BaseEntity
    {
        public int SportId { get; set; }
        public int GeneralFacilityId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
