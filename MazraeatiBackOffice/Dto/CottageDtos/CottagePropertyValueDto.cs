using MazraeatiBackOffice.Dto.SportDtos;
using System.Collections.Generic;

namespace MazraeatiBackOffice.Dto.CottageDtos
{
    public class CottagePropertyValueDto
    {
        public int Id { get; set; }
        public int CottageId { get; set; }
        public int PropertyTemplateId { get; set; }
        public string PropertyKey { get; set; }
        public string ValueText { get; set; }
        public bool? ValueBool { get; set; }
        public int? ValueOptionId { get; set; }
        public int PropertyType { get; set; }
        public string PropertyLabelAr { get; set; }
        public bool IsRequired { get; set; }
        public List<CottagePropertyOptionDto> Options { get; set; } = new List<CottagePropertyOptionDto>();
    }
}
