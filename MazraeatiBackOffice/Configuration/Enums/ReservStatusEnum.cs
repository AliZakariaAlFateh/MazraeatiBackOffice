using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Configuration.Enums
{
    public enum ReservStatusEnum
    {
        [Display(Name = "قيد الانتظار")]
        Pending = 0,
        [Display(Name = "تم الموافقة")]
        Accepted = 1,
        [Display(Name = "مؤكد")]
        Confirmed = 2,
        [Display(Name = "ملغى")]
        Cancelled = 3
    }
}
