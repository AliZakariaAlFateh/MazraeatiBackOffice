using System;

namespace MazraeatiBackOffice.Models
{
    public class ScreenViewModel
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string ScreenUrl { get; set; }
        public string Icon { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public bool IsActive { get; set; }
        public bool IsMenu { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
