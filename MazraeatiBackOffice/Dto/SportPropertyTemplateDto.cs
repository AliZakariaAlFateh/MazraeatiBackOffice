using System.Collections.Generic;

namespace MazraeatiBackOffice.Dto
{
    public class SportPropertyTemplateDto
    {
        public int Id { get; set; }
        public int SportTypeId { get; set; }
        public string PropertyKey { get; set; }
        public string PropertyLabelAr { get; set; }
        public string PropertyLabelEn { get; set; }
        public int PropertyType { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public List<SportPropertyOptionDto> Options { get; set; } = new List<SportPropertyOptionDto>();
    }
}
