using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("DeviceToken")]
    public class DeviceToken : BaseEntity
    {
        public string? DeviceId { get; set; }
        public string? Token { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
