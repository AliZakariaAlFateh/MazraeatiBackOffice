using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportImages")]
    public class SportImage:BaseEntity
    {
        public int SportId { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public bool Vip { get; set; }
        public bool Active { get; set; } = true;
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}
