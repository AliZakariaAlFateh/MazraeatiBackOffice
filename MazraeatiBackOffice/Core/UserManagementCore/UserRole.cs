using MazraeatiBackOffice.Core.SystemCore;

namespace MazraeatiBackOffice.Core.UserManagementCore
{
    public class UserRole:BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        // Navigation properties
        public virtual AdminUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}
