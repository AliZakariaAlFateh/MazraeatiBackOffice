using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerUserModel
    {
        public int Id { get; set; }

        [DisplayName("رمز الدخول")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string UserName { get; set; }

        [DisplayName("كلمة السر")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string Password { get; set; }
        public int FarmerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
