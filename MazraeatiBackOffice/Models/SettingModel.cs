using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Models
{
    public class SettingModel
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Name { get; set; }

        [DisplayName("قيمة المتغير")]
        [Required(ErrorMessage = "برجاء تعبئة الحقل")]
        public string Value { get; set; }
        public bool IsEditior { get; set; }
        public bool IsMedia { get; set; }
        [DisplayName("اسم المتغير")]
        public string DisplayName { get; set; }
    }
}
