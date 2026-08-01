using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportAdditionalServices")]
    public class SportAdditionalService:BaseEntity
    {
        public int SportId { get; set; }
        public int AdditionalServiceId { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
