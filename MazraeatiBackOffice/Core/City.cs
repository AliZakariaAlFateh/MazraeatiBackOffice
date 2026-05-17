using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core
{
    [Table("City")]
    public class City : BaseEntity
    {
        public int CountryId { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public bool Active { get; set; }
    }
}
