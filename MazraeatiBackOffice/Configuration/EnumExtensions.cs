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

        public static string ToArabicUserAppOrDashboard(this IsMahjouzAppOrDashboard? value)
        {
            return value switch
            {
                IsMahjouzAppOrDashboard.Dashboard => "لوحة التحكم",
                IsMahjouzAppOrDashboard.MahjouzApp => "تطبيق الموبايل",
                _ => "غير معروف"
            };
        }


        //public static string ToArabic(this FarmAppUserStatus? status)
        //{
        //    return status.HasValue ? status.Value.ToArabic() : "غير معروف";
        //}

        //public static string GetStatusClass(this int status)
        //{
        //    return status switch
        //    {
        //        0 => "badge badge-success",   // Active
        //        1 => "badge badge-danger",    // Inactive
        //        2 => "badge badge-warning",   // Maintenance
        //        _ => "badge badge-secondary"
        //    };
        //}

        //public static string GetStatusIcon(this int status)
        //{
        //    return status switch
        //    {
        //        0 => "zmdi zmdi-check-circle",
        //        1 => "zmdi zmdi-close-circle",
        //        2 => "zmdi zmdi-settings",
        //        _ => "zmdi zmdi-help"
        //    };
        //}

    }
}
