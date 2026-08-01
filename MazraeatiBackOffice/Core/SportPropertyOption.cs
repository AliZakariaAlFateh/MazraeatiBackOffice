using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
[Table("SportPropertyOptions")]
    public class SportPropertyOption : BaseEntity
    {
        [Required]
        public int PropertyTemplateId { get; set; }

        [Required]
        [MaxLength(200)]
        public string OptionValue { get; set; }

        [Required]
        [MaxLength(200)]
        public string OptionTextAr { get; set; }

        [MaxLength(200)]
        public string OptionTextEn { get; set; }

        public int SortOrder { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("PropertyTemplateId")]
        public virtual SportPropertyTemplate PropertyTemplate { get; set; }
    }
}
