using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core
{
    [Table("Lookup")]
    public class Lookup : BaseEntity
    {
        public string LookupCode { get; set; }
    }
}
