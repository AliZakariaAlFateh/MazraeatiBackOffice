using System;

namespace MazraeatiBackOffice.Core
{
    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScreenId { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
        public bool CanExport { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual AdminUser User { get; set; }
        public virtual Screen Screen { get; set; }
    }
}
