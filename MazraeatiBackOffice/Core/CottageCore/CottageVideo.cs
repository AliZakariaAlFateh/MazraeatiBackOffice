using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.CottageCore
{
    [Table("CottageVideos")]
    public class CottageVideo:BaseEntity
    {
        public int CottageId { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; } = true;
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}
