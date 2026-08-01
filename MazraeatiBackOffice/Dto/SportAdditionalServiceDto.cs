namespace MazraeatiBackOffice.Dto
{
    public class SportAdditionalServiceDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int AdditionalServiceId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
