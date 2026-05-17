using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("NotificationsFarm")]
    public class NotificationsFarm:BaseEntity
    {
        public string FarmName { get; set; }
        public string LocationDescription { get; set; }
        public DateTime TimeDate { get; set; }
    }
}
