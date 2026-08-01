namespace MazraeatiBackOffice.Dto
{
    public class SportFeatureDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int TypeId { get; set; }
        public string FeatureText { get; set; }
        public string FeatureTextEn { get; set; }
        public bool IsCheck { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
}
