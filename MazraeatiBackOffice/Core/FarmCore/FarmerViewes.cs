using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.FarmCore
{
    [Table("FarmerViewes")]
    public class FarmerViewes:BaseEntity
    {
        public int FarmerId { get; set; }
        public string DeviceId { get; set; }
    }
}
