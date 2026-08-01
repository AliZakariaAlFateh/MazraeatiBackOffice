using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Configuration
{
    public enum PropertyTypeEnum
    {
        [Display(Name = "نص")]
        Text = 1,
        [Display(Name = "رقم")]
        Number = 2,
        [Display(Name = "اختيار (Checkbox)")]
        Checkbox = 3,
        [Display(Name = "قائمة منسدلة")]
        Dropdown = 4,
        [Display(Name = "نص طويل")]
        TextArea = 5,
        [Display(Name = "اختيار من متعدد (RadioButton)")]
        RadioButton = 6
    }
}
