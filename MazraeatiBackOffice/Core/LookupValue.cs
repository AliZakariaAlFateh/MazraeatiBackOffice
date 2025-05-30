using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core
{
    [Table("LookupValue")]
    public class LookupValue : BaseEntity
    {
        public int LookupId { get; set; }
        public string Code { get; set; }
        public string ValueEn { get; set; }
        public string ValueAr { get; set; }
    }
}
