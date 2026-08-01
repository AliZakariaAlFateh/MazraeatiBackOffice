using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("AdminUsers")]
    public class AdminUser:BaseEntity
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSuperAdmin { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string ProfileImage { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<UserLog> UserLogs { get; set; }
    }
}
