using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.CottageCore
{
    [Table("CottageCottageGeneralFacilities")]
    public class CottageCottageGeneralFacility:BaseEntity
    {
        public int CottageId { get; set; }
        public int GeneralFacilityId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
