namespace MazraeatiBackOffice.Models
{
    public class UserPermissionViewModel
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenUrl { get; set; }
        public string Icon { get; set; }
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMenu { get; set; }

        // الصلاحيات
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanExport { get; set; }
    }
}
