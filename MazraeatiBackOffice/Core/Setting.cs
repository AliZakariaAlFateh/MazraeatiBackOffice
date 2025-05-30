using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("Setting")]
    public class Setting : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsEditior { get; set; }
        public bool IsMedia { get; set; }
        public string DisplayName { get; set; }
    }
}
