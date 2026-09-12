using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.UserManagementCore
{
    [Table("Customer")]
    public class Customer: BaseEntity
    {
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
        public string DeviceId { get; set; }
        public string DeviceToken { get; set; }
    }
}
