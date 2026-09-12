using MazraeatiBackOffice.Dto.SportDtos;
using System.Collections.Generic;

namespace MazraeatiBackOffice.Dto.CottageDtos
{
    public class CottagePropertyTemplateDto
    {
        public int Id { get; set; }
        public string PropertyKey { get; set; }
        public string PropertyLabelAr { get; set; }
        public string PropertyLabelEn { get; set; }
        public int PropertyType { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public List<CottagePropertyOptionDto> Options { get; set; } = new List<CottagePropertyOptionDto>();
    }
}
