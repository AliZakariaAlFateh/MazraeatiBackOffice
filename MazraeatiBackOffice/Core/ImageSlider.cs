using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Core
{
    [Table("ImageSlider")]
    public class ImageSlider : BaseEntity
    {
        public int CountryId { get; set; }
        public string PageName { get; set; }
        public string Image { get; set; }
        public string ExtraText { get; set; }
        public string ExtraTextEn { get; set; }

        public string RedirectLink { get; set; }
        public string Target { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; }
        public bool Active { get; set; }
    }
}
