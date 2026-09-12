using MazraeatiBackOffice.Configuration.Enums;
using MazraeatiBackOffice.Core.SportCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.CottageCore
{
    [Table("CottagePropertyTemplates")]
    public class CottagePropertyTemplate:BaseEntity
    {

        [Required]
        [MaxLength(100)]
        public string PropertyKey { get; set; }

        [Required]
        [MaxLength(200)]
        public string PropertyLabelAr { get; set; }

        [MaxLength(200)]
        public string PropertyLabelEn { get; set; }

        [Required]
        public PropertyTypeEnum PropertyType { get; set; }

        public bool IsRequired { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }


        public virtual ICollection<CottagePropertyOption> Options { get; set; }
        public virtual ICollection<CottagePropertyValue> PropertyValues { get; set; }
    }
}
