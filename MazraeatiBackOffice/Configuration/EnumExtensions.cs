namespace MazraeatiBackOffice.Configuration
{
    public static class EnumExtensions
    {
        public static string ToArabic(this FarmAppUserStatus? status)
        {
            return status switch
            {
                FarmAppUserStatus.Active => "نشط",
                FarmAppUserStatus.Inactive => "غير نشط",
                FarmAppUserStatus.Maintenance => "صيانة",
                _ => "غير معروف"
            };
        }


        //public static string ToArabic(this FarmAppUserStatus? status)
        //{
        //    return status.HasValue ? status.Value.ToArabic() : "غير معروف";
        //}
    }
}
