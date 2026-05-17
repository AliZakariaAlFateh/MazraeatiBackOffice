using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerViewes")]
    public class FarmerViewes:BaseEntity
    {
        public int FarmerId { get; set; }
        public string DeviceId { get; set; }
    }
}
