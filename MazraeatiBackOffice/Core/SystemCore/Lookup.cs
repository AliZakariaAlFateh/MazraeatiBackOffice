using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core.SystemCore
{
    [Table("Lookup")]
    public class Lookup : BaseEntity
    {
        public string LookupCode { get; set; }
    }
}
