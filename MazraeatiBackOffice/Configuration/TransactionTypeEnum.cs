using System.ComponentModel.DataAnnotations;

namespace MazraeatiBackOffice.Configuration
{
    /// <summary>
    /// أنواع حركات النقاط
    /// </summary>
    public enum TransactionTypeEnum
    {
        [Display(Name = "إضافة نقاط")]
        Earn = 1,
        [Display(Name = "خصم نقاط")]
        Redeem = 2,
        [Display(Name = "انتهاء صلاحية")]
        Expire = 3,
        [Display(Name = "تعديل يدوي")]
        Adjust = 4
    }
}
