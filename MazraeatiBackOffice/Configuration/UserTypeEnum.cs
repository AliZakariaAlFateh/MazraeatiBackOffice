
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Configuration
{
    public enum UserTypeEnum
    {
        //[Display(Name = "عام")]
        //General = 0,
        [Display(Name = "مالك مزرعة")]
        Farmer = 0,
        [Display(Name = "مالك ملعب كرة قدم")]
        Football = 1,
        [Display(Name = "مالك ملعب بادل")]
        Padel = 2,
        [Display(Name = "مالك ملعب تنس")]
        Tennis = 3,
        [Display(Name = "مالك ملعب كرة سلة")]
        Basketball = 4,
        [Display(Name = "مالك ملعب كرة طائرة")]
        Volleyball = 5,
        [Display(Name = "مالك مسبح")]
        Swimming = 6,
        [Display(Name = "مالك مركز فروسية")]
        Equestrian = 7,
        [Display(Name = "مالك ميدان رماية")]
        Shooting = 8,
        [Display(Name = "مالك Pickleball")]
        Pickleball = 9,
        [Display(Name = "مالك تنس طاولة")]
        TableTennis = 10,
        [Display(Name = "مالك اسكواش")]
        Squash = 11,
        [Display(Name = "مالك ريشة طائرة")]
        Badminton = 12
    }
}
