namespace MazraeatiBackOffice.Core
{
    public class TripExtraFeatureTypeDto 
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int TypeId { get; set; }
        public string DescAr { get; set; }
        public string ExtraText { get; set; }
        public bool IsCheck { get; set; }
    }
}
