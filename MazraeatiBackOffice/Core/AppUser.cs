using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("AppUser")]
    public class AppUser: BaseEntity
    {
        public string UserName { get; set; }
        public string MobilePhone { get; set; } // Whatapp Number  .....
        public string MobileNumber { get; set; }
        public string PasswordHash { get; set; }
    }
}
