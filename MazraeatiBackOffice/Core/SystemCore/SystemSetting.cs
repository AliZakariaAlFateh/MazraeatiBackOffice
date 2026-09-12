using System;

namespace MazraeatiBackOffice.Core.SystemCore
{
    public class SystemSetting:BaseEntity
    {
        //public int Id { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string SettingType { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
