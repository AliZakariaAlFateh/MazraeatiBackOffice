using System.ComponentModel;

namespace MazraeatiBackOffice.Models
{
    public class LookupModel
    {
        public int Id { get; set; }

        [DisplayName("نوع المزايا")]
        public string LookupCode { get; set; }
        public string LookupCodeDesc { get; set; }
    }
}
