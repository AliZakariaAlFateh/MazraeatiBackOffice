using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MazraeatiBackOffice.Models
{
    public class termsModel
    {
        public int Id { get; set; }

        [DisplayName("وصف البند بالعربي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescAr { get; set; }

        [DisplayName("وصف البند بالانجليزي")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string DescEn { get; set; }
    }
}
