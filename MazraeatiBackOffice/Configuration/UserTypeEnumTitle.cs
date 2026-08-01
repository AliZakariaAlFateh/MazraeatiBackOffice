
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Configuration
{
    public enum UserTypeEnumTitle
    {
        //[Display(Name = "عام")]
        //General = 0,
        [Display(Name = "مزرعة")]
        Farmer = 0,
        [Display(Name = "ملعب كرة قدم")]
        Football = 1,
        [Display(Name = "ملعب بادل")]
        Padel = 2,
        [Display(Name = "ملعب تنس")]
        Tennis = 3,
        [Display(Name = "ملعب كرة سلة")]
        Basketball = 4,
        [Display(Name = "ملعب كرة طائرة")]
        Volleyball = 5,
        [Display(Name = "مسبح")]
        Swimming = 6,
        [Display(Name = "مركز فروسية")]
        Equestrian = 7,
        [Display(Name = "ميدان رماية")]
        Shooting = 8,
        [Display(Name = "Pickleball")]
        Pickleball = 9,
        [Display(Name = "تنس طاولة")]
        TableTennis = 10,
        [Display(Name = "اسكواش")]
        Squash = 11,
        [Display(Name = "ريشة طائرة")]
        Badminton = 12
    }
}
