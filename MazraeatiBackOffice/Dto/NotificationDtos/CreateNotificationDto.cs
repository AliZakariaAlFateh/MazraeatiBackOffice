namespace MazraeatiBackOffice.Dto.NotificationDtos
{
    public class CreateNotificationDto
    {
        public string Type { get; set; } // 'Farm', 'Price'
        public string Title { get; set; }
        public string Message { get; set; }
        public object OldData { get; set; }
        public object NewData { get; set; }
        public int? CreatedBy { get; set; }
    }
}
