using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core
{
    [Table("Country")]
    public class Country : BaseEntity
    {
        public string Code { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public string ImageUrl { get; set; }
        public bool Active { get; set; }
    }
}
