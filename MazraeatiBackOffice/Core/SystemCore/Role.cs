using MazraeatiBackOffice.Core.UserManagementCore;
using System.Collections.Generic;

namespace MazraeatiBackOffice.Core.SystemCore
{
    public class Role:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
