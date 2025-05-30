using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("Onboarding")]
    public class Onboarding : BaseEntity
    {
        public int CountryId { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string ExtraText { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
