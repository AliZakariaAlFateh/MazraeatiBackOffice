using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MazraeatiBackOffice.Core
{
    [Table("FarmerUser")]
    public class FarmerUser : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int FarmerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
