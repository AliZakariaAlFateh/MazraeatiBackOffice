namespace MazraeatiBackOffice.Dto.CottageDtos
{
    public class CottageGeneralFacilityDto
    {
        public int Id { get; set; }
        public int CottageId { get; set; }
        public int FacilityId { get; set; }
        public string FacilityText { get; set; }
        public string FacilityTextEn { get; set; }
        public bool IsCheck { get; set; }
    }
}
