namespace MazraeatiBackOffice.Dto
{
    public class SportSportFeatureDto
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int SportFeatureId { get; set; }
        public bool IsChecked { get; set; } = false;
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }
}
