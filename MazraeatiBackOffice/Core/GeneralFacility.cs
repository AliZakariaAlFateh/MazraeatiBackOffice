using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("GeneralFacilities")]
    public class GeneralFacility:BaseEntity
    {
        public string FacilityTextAr { get; set; }
        public string FacilityTextEn { get; set; }
        public string IconClass { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
