using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("terms")]
    public class terms :BaseEntity
    {
        public string DescAr { get; set; }
        public string DescEn { get; set; }

    }

   
}
