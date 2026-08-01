using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class LoyaltyBookingActivityModel
    {
        public int Id { get; set; }

        [Display(Name = "نوع الحجز")]
        [Required(ErrorMessage = "نوع الحجز مطلوب")]
        public string BookingType { get; set; }

        [Display(Name = "نوع النشاط")]
        [Required(ErrorMessage = "نوع النشاط مطلوب")]
        public int ActivityTypeId { get; set; }

        [Display(Name = "اسم النشاط")]
        public string ActivityTypeName { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;
    }
}
