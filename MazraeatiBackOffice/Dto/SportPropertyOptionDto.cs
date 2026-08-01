namespace MazraeatiBackOffice.Dto
{
    public class SportPropertyOptionDto
    {
        public int Id { get; set; }
        public int PropertyTemplateId { get; set; }
        public string OptionValue { get; set; }
        public string OptionTextAr { get; set; }
        public string OptionTextEn { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
