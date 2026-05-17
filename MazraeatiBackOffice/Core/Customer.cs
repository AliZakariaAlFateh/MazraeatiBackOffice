using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("Customer")]
    public class Customer: BaseEntity
    {
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
    }
}
