using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerFeedback")]
    public class FarmerFeedback : BaseEntity
    {
        public int FarmerId { get; set; }
        public int FeedbackId { get; set; }
        public string Note { get; set; }
        public bool IsSolved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
