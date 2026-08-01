using MazraeatiBackOffice.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportPropertyTemplates")]
    public class SportPropertyTemplate : BaseEntity
    {
        [Required]
        public int SportTypeId { get; set; }

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

        [ForeignKey("SportTypeId")]
        public virtual SportType SportType { get; set; }

        public virtual ICollection<SportPropertyOption> Options { get; set; }
        public virtual ICollection<SportPropertyValue> PropertyValues { get; set; }
    }
}
