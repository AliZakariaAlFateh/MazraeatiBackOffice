using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models
{
    public class EditUserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "يجب ادخال الاسم")]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "اسم المستحدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "يجب ادخال الاسم كاملا")]
        [StringLength(200, MinimumLength = 3)]
        [Display(Name = "اسم المستخدم بالكامل")]
        public string FullName { get; set; }

        [EmailAddress]
        [Display(Name = "الإيميل")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        public List<int> RoleIds { get; set; }
        public List<RoleCheckboxModel> AvailableRoles { get; set; }
    }
}
