using MazraeatiBackOffice.Configuration;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("AppUser")]
    public class AppUser: BaseEntity
    {
        //public string UserName { get; set; }
        //public string MobilePhone { get; set; } // Whatapp Number  .....
        //public string MobileNumber { get; set; }
        //public string PasswordHash { get; set; }
        //public UserTypeEnum UserType { get; set; } = UserTypeEnum.Farmer;
        //public bool IsActive { get; set; } = false;
        //public bool IsDeleted { get; set; } = false;

        //New multiple activites
        public string UserName { get; set; }
        public string MobilePhone { get; set; }
        public string MobileNumber { get; set; }
        public string PasswordHash { get; set; }

        // ===== تتغير من int إلى string =====
        public string UserType { get; set; } = "0"; // "0,1,2" للمالك المتعدد
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        internal object FirstOrDefault()
        {
            throw new NotImplementedException();
        }
    }
}
