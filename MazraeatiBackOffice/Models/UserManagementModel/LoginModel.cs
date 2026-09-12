using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.UserManagementModel
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
