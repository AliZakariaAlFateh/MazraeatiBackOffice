using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class CommonQuestionsVisitorsModel
    {
        public int Id { get; set; }

        [DisplayName("السؤال باللغة العربية")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public string QuestAr { get; set; }
        [DisplayName("الإجابة باللغة العربية")]
        public string AnswerAr { get; set; }
        [DisplayName("السؤال باللغة الإنجليزية")]
        public string QuestEn { get; set; }
        [DisplayName("الإجابة باللغة الإنجليزية")]
        public string AnswerEn { get; set; }
        [DisplayName("رابط الصورة")]
        public string ImageUrl { get; set; }

    }
}
