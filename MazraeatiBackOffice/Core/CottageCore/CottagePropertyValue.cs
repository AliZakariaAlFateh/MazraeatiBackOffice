using MazraeatiBackOffice.Core.SportCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core.CottageCore
{
    [Table("CottagePropertyValues")]
    public class CottagePropertyValue:BaseEntity
    {
        [Required]
        public int CottageId { get; set; }

        [Required]
        public int PropertyTemplateId { get; set; }

        public string ValueText { get; set; }

        public bool? ValueBool { get; set; }

        public int? ValueOptionId { get; set; }

        [ForeignKey("CottageId")]
        public virtual Cottage Cottage { get; set; }

        [ForeignKey("PropertyTemplateId")]
        public virtual CottagePropertyTemplate PropertyTemplate { get; set; }

        [ForeignKey("ValueOptionId")]
        public virtual CottagePropertyOption ValueOption { get; set; }
    }
}
