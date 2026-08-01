using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportVideos")]
    public class SportVideo:BaseEntity
    {
        public int SportId { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; } = true;
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}
