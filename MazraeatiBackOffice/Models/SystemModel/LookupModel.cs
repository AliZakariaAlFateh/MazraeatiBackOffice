using System.ComponentModel;

namespace MazraeatiBackOffice.Models.SystemModel
{
    public class LookupModel
    {
        public int Id { get; set; }

        [DisplayName("نوع المزايا")]
        public string LookupCode { get; set; }
        public string LookupCodeDesc { get; set; }
    }
}
