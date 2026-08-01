using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("CommonQuestionsVisitors")]
    public class CommonQuestionsVisitors:BaseEntity
    {
        public string QuestAr { get; set; }
        public string AnswerAr { get; set; }
        public string QuestEn { get; set; }
        public string AnswerEn { get; set; }
        public string ImageUrl { get; set; }

    }
}
