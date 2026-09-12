namespace MazraeatiBackOffice.Dto.CottageDtos
{
    public class CottageCottageGeneralFacilityDto
    {
        public int Id { get; set; }
        public int CottageId { get; set; }
        public int GeneralFacilityId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
