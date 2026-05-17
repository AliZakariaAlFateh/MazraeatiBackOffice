using System.ComponentModel.DataAnnotations.Schema;

namespace MazraeatiBackOffice.Core
{
    [Table("AppUserBlackList")]
    public class AppUserBlackList : BaseEntity
    {
        public string CustMobileNum { get; set; }
        public string CustName { get; set; }
        public string CustNameEn { get; set; }

        public string Reason { get; set; }
        public string ReasonEn { get; set; }

        public string ImageUrl { get; set; }
        public bool IsApprove { get; set; }
        public int UserId { get; set; }
        public bool IsBlocked { get; set; }
    }
}
