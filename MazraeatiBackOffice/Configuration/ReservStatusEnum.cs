using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Configuration
{
    public enum ReservStatusEnum
    {
        [Display(Name = "قيد الانتظار")]
        Pending = 0,
        [Display(Name = "مؤكد")]
        Confirmed = 1,
        [Display(Name = "ملغى")]
        Cancelled = 2
    }
}
