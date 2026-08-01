using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("AdditionalServices")]
    public class AdditionalService:BaseEntity
    {
        public string ServiceTextAr { get; set; }
        public string ServiceTextEn { get; set; }
        public string IconClass { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
