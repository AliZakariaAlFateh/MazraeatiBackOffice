using System.ComponentModel;

namespace MazraeatiBackOffice.Models
{
    public class LookupValueModel
    {
        public int Id { get; set; }

        [DisplayName("نوع المزايا")]
        public int LookupId { get; set; }
        public string LookupDesc { get; set; }

        [DisplayName("الرمز")]
        public string Code { get; set; }

        [DisplayName("وصف بالانجليزي")]
        public string ValueEn { get; set; }

        [DisplayName("وصف بالعربي")]
        public string ValueAr { get; set; }
    }
}
