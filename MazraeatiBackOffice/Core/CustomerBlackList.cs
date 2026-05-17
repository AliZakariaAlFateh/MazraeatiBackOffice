using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("CustomerBlackList")]
    public class CustomerBlackList : BaseEntity
    {
        public string CustMobileNum { get; set; }
        public string CustName { get; set; }
        public string CustNameEn { get; set; }

        public string Reason { get; set; }
        public string ReasonEn { get; set; }

        public string ImageUrl { get; set; }
        public bool IsApprove { get; set; }
    }
}
