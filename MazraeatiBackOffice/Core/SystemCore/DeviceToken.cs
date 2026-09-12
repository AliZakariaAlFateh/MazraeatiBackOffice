using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.SystemCore
{
    [Table("DeviceToken")]
    public class DeviceToken : BaseEntity
    {
        public string DeviceId { get; set; }
        public int UserId { get; set; } //new .....
        public string Token { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
