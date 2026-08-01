using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("SportPropertyValues")]
    public class SportPropertyValue : BaseEntity
    {
        [Required]
        public int SportId { get; set; }

        [Required]
        public int PropertyTemplateId { get; set; }

        public string ValueText { get; set; }

        public bool? ValueBool { get; set; }

        public int? ValueOptionId { get; set; }

        [ForeignKey("SportId")]
        public virtual Sport Sport { get; set; }

        [ForeignKey("PropertyTemplateId")]
        public virtual SportPropertyTemplate PropertyTemplate { get; set; }

        [ForeignKey("ValueOptionId")]
        public virtual SportPropertyOption ValueOption { get; set; }
    }
}
