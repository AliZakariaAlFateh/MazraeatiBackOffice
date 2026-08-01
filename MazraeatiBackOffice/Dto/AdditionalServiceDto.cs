namespace MazraeatiBackOffice.Dto
{
    public class AdditionalServiceDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceText { get; set; }
        public string ServiceTextEn { get; set; }
        public bool IsCheck { get; set; }
    }
}
