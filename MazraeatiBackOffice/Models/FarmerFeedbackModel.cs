using MazraeatiBackOffice.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class FarmerFeedbackModel
    {
        public int Id { get; set; }
        public int FarmerId { get; set; }

        [DisplayName("نوع التقييم")]
        [Required(ErrorMessage = "يرجى تعبئه الحقل")]
        public int FeedbackId { get; set; }
        public string FeedbackTypeDesc { get; set; }

        [DisplayName("ملاحظات")]
        public string Note { get; set; }

        [DisplayName("هل تم حل المشكلة")]
        public bool IsSolved { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<LookupValue> LookupValues { get; set; }
    }
}
