namespace MazraeatiBackOffice.Dto
{
    public class SportGeneralFacilityDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int GeneralFacilityId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
