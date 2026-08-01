using MazraeatiBackOffice.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Models.LoyaltyPoints
{
    public class LoyaltyTransactionModel
    {
        public long Id { get; set; }

        [Display(Name = "العميل")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        [Display(Name = "نوع الحركة")]
        public TransactionTypeEnum TransactionType { get; set; }

        [Display(Name = "النقاط")]
        public int Points { get; set; }

        [Display(Name = "المرجع")]
        public int? ReferenceId { get; set; }

        [Display(Name = "نوع المرجع")]
        public string ReferenceType { get; set; }

        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Display(Name = "التاريخ")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "تاريخ الانتهاء")]
        public DateTime? ExpireDate { get; set; }

        [Display(Name = "تم بواسطة")]
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }
}
