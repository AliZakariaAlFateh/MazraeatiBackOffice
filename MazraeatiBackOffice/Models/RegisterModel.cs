using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال الاسم")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
        [Display(Name = "اسم المستحدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "يجب ادخال الاسم بالكامل")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 200 characters")]
        [Display(Name = "اسم المستخدم بالكامل")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "الإيميل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب ادخال كلمة المرور")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "الصلاحيات")]
        public List<int> RoleIds { get; set; }

        public List<RoleCheckboxModel> AvailableRoles { get; set; }
    }
}
