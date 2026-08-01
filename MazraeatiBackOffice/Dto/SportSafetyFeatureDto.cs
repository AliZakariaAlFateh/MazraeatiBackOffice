namespace MazraeatiBackOffice.Dto
{
    public class SportSafetyFeatureDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }

        public int SafetyFeatureId { get; set; }

        public bool IsChecked { get; set; } = false;
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
}
