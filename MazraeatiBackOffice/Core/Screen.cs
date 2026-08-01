using System;
using System.Collections.Generic;

namespace MazraeatiBackOffice.Core
{
    public class Screen
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string ScreenUrl { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsMenu { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Screen Parent { get; set; }
        public virtual ICollection<Screen> SubScreens { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
