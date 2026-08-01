namespace MazraeatiBackOffice.Dto
{
    public class SafetyFeatureDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int TypeId { get; set; } // SafetyFeatureId
        public string FeatureText { get; set; }
        public string FeatureTextEn { get; set; }
        public bool IsCheck { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
}
