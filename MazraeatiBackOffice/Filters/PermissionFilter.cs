using Google;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Filters
{
    //public class PermissionFilter : IAsyncAuthorizationFilter
    //{
    //    private readonly DataContext _context;
    //    private readonly IUserService _userService;

    //    // قائمة الصفحات العامة (متاحة للجميع بدون تسجيل دخول)
    //    private readonly string[] _publicPages = new string[]
    //    {
    //        "/Account/Login",
    //        "/Account/AccessDenied",
    //        "/Account/Logout"
    //    };

    //    public PermissionFilter(DataContext context, IUserService userService)
    //    {
    //        _context = context;
    //        _userService = userService;
    //    }

    //    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    //    {
    //        var user = context.HttpContext.User;

    //        // =============================================
    //        // 1. استثناء الـ Account Controller بالكامل (الأهم)
    //        // =============================================
    //        var controller = context.RouteData.Values["controller"]?.ToString();
    //        var action = context.RouteData.Values["action"]?.ToString();

    //        // لو الـ Controller هو Account، اخرج فوراً (ممنوع أي تحقق)
    //        if (controller?.ToLower() == "account")
    //        {
    //            return;
    //        }

    //        // =============================================
    //        // 2. استثناء الصفحات العامة
    //        // =============================================
    //        var path = context.HttpContext.Request.Path.ToString();
    //        var baseUrl = path.Split('?')[0];

    //        if (_publicPages.Any(p => baseUrl.Equals(p, StringComparison.OrdinalIgnoreCase)))
    //        {
    //            return;
    //        }

    //        // =============================================
    //        // 3. التحقق من تسجيل الدخول
    //        // =============================================
    //        if (!user.Identity.IsAuthenticated)
    //        {
    //            context.Result = new RedirectResult("/Account/Login");
    //            return;
    //        }

    //        // =============================================
    //        // 4. جلب الـ UserId
    //        // =============================================
    //        var userIdClaim = user.FindFirst("UserId")?.Value;
    //        if (!int.TryParse(userIdClaim, out int userId))
    //        {
    //            context.Result = new RedirectResult("/Account/Login");
    //            return;
    //        }

    //        // =============================================
    //        // 5. التحقق من SuperAdmin
    //        // =============================================
    //        var isSuperAdmin = await _context.AdminUsers
    //            .AnyAsync(u => u.Id == userId && u.IsSuperAdmin);

    //        if (isSuperAdmin)
    //        {
    //            return; // SuperAdmin يدخل كل حاجة
    //        }

    //        // =============================================
    //        // 6. جلب الـ Screen
    //        // =============================================
    //        var screen = await GetScreenFromUrl(baseUrl, controller, action);

    //        // لو مفيش Screen مسجل، نسمح بالدخول
    //        if (screen == null)
    //        {
    //            context.Result = new RedirectResult("/Account/AccessDenied");
    //            return;
    //        }

    //        // =============================================
    //        // 7. جلب صلاحية المستخدم
    //        // =============================================
    //        var permission = await _context.UserPermissions
    //            .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screen.Id);

    //        if (permission == null)
    //        {
    //            context.Result = new RedirectResult("/Account/AccessDenied");
    //            return;
    //        }

    //        // =============================================
    //        // 8. التحقق من الصلاحية حسب نوع الـ Action
    //        // =============================================
    //        var actionType = GetActionType(action);
    //        bool hasPermission = false;

    //        switch (actionType)
    //        {
    //            case "View":
    //                hasPermission = permission.CanView;
    //                break;
    //            case "Create":
    //                hasPermission = permission.CanCreate;
    //                break;
    //            case "Edit":
    //                hasPermission = permission.CanEdit;
    //                break;
    //            case "Delete":
    //                hasPermission = permission.CanDelete;
    //                break;
    //            case "Export":
    //                hasPermission = permission.CanExport;
    //                break;
    //            default:
    //                hasPermission = permission.CanView;
    //                break;
    //        }

    //        if (!hasPermission)
    //        {
    //            context.Result = new RedirectResult("/Account/AccessDenied");
    //        }
    //    }

    //    private async Task<Screen> GetScreenFromUrl(string baseUrl, string controller, string action)
    //    {
    //        // 1. جلب بالـ URL الكامل
    //        var screen = await _context.Screens
    //            .FirstOrDefaultAsync(s => s.ScreenUrl == baseUrl);

    //        if (screen != null)
    //            return screen;

    //        // 2. جلب بالـ Controller/Action
    //        if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
    //        {
    //            var controllerAction = $"/{controller}/{action}";
    //            screen = await _context.Screens
    //                .FirstOrDefaultAsync(s => s.ScreenUrl.Contains(controllerAction));
    //        }

    //        // 3. جلب بـ Wildcard (*)
    //        if (screen == null)
    //        {
    //            var screens = await _context.Screens
    //                .Where(s => s.ScreenUrl.Contains("*"))
    //                .ToListAsync();

    //            foreach (var s in screens)
    //            {
    //                var pattern = s.ScreenUrl.Replace("*", "");
    //                if (baseUrl.StartsWith(pattern))
    //                {
    //                    screen = s;
    //                    break;
    //                }
    //            }
    //        }

    //        return screen;
    //    }

    //    private string GetActionType(string action)
    //    {
    //        if (string.IsNullOrEmpty(action))
    //            return "View";

    //        var actionLower = action.ToLower();

    //        var createActions = new[] { "create", "add", "new", "insert", "register" };
    //        var editActions = new[] { "edit", "update", "modify", "change" };
    //        var deleteActions = new[] { "delete", "remove", "destroy" };
    //        var exportActions = new[] { "export", "download", "print" };

    //        if (createActions.Any(a => actionLower.Contains(a) || actionLower == a))
    //            return "Create";

    //        if (editActions.Any(a => actionLower.Contains(a) || actionLower == a))
    //            return "Edit";

    //        if (deleteActions.Any(a => actionLower.Contains(a) || actionLower == a))
    //            return "Delete";

    //        if (exportActions.Any(a => actionLower.Contains(a) || actionLower == a))
    //            return "Export";

    //        return "View";
    //    }
    //}



    public class PermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        // قائمة الصفحات العامة (متاحة للجميع بدون تسجيل دخول)
        private readonly string[] _publicPages = new string[]
        {
        "/Account/Login",
        "/Account/AccessDenied",
        "/Account/Logout"
        };

        public PermissionFilter(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // =============================================
            // 1. استثناء الـ Account Controller بالكامل (الأهم)
            // =============================================
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // لو الـ Controller هو Account، اخرج فوراً (ممنوع أي تحقق)
            if (controller?.ToLower() == "account")
            {
                return;
            }

            // =============================================
            // 2. استثناء الصفحات العامة
            // =============================================
            var path = context.HttpContext.Request.Path.ToString();
            var baseUrl = path.Split('?')[0];

            if (_publicPages.Any(p => baseUrl.Equals(p, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            // =============================================
            // 3. التحقق من طلب AJAX (إضافة جديدة)
            // =============================================
            var isAjax = IsAjaxRequest(context.HttpContext.Request);

            // =============================================
            // 4. التحقق من تسجيل الدخول
            // =============================================
            if (!user.Identity.IsAuthenticated)
            {
                if (isAjax)
                {
                    context.Result = new JsonResult(new
                    {
                        success = false,
                        message = "يرجى تسجيل الدخول أولاً",
                        redirect = "/Account/Login"
                    });
                    return;
                }
                context.Result = new RedirectResult("/Account/Login");
                return;
            }

            // =============================================
            // 5. جلب الـ UserId
            // =============================================
            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
            {
                if (isAjax)
                {
                    context.Result = new JsonResult(new
                    {
                        success = false,
                        message = "انتهت الجلسة، يرجى تسجيل الدخول مرة أخرى",
                        redirect = "/Account/Login"
                    });
                    return;
                }
                context.Result = new RedirectResult("/Account/Login");
                return;
            }

            // =============================================
            // 6. التحقق من SuperAdmin
            // =============================================
            var isSuperAdmin = await _context.AdminUsers
                .AnyAsync(u => u.Id == userId && u.IsSuperAdmin);

            if (isSuperAdmin)
            {
                return; // SuperAdmin يدخل كل حاجة
            }

            // =============================================
            // 7. جلب الـ Screen
            // =============================================
            var screen = await GetScreenFromUrl(baseUrl, controller, action);

            // لو مفيش Screen مسجل، نسمح بالدخول
            if (screen == null)
            {
                if (isAjax)
                {
                    context.Result = new JsonResult(new
                    {
                        success = false,
                        message = "الصفحة غير مسجلة في النظام",
                        errorType = "permission_denied"
                    });
                    return;
                }
                context.Result = new RedirectResult("/Account/AccessDenied");
                return;
            }

            // =============================================
            // 8. جلب صلاحية المستخدم
            // =============================================
            var permission = await _context.UserPermissions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screen.Id);

            if (permission == null)
            {
                if (isAjax)
                {
                    context.Result = new JsonResult(new
                    {
                        success = false,
                        message = "ليس لديك صلاحية للوصول إلى هذه الصفحة",
                        errorType = "permission_denied"
                    });
                    return;
                }
                context.Result = new RedirectResult("/Account/AccessDenied");
                return;
            }

            // =============================================
            // 9. التحقق من الصلاحية حسب نوع الـ Action
            // =============================================
            var actionType = GetActionType(action);
            bool hasPermission = false;

            switch (actionType)
            {
                case "View":
                    hasPermission = permission.CanView;
                    break;
                case "Create":
                    hasPermission = permission.CanCreate;
                    break;
                case "Edit":
                    hasPermission = permission.CanEdit;
                    break;
                case "Delete":
                    hasPermission = permission.CanDelete;
                    break;
                case "Export":
                    hasPermission = permission.CanExport;
                    break;
                default:
                    hasPermission = permission.CanView;
                    break;
            }

            if (!hasPermission)
            {
                if (isAjax)
                {
                    context.Result = new JsonResult(new
                    {
                        success = false,
                        message = "ليس لديك صلاحية للقيام بهذا الإجراء",
                        errorType = "permission_denied"
                    });
                    return;
                }
                context.Result = new RedirectResult("/Account/AccessDenied");
            }
        }

        // =============================================
        // التحقق من طلب AJAX (إضافة جديدة)
        // =============================================
        private bool IsAjaxRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private async Task<Screen> GetScreenFromUrl(string baseUrl, string controller, string action)
        {
            // 1. جلب بالـ URL الكامل
            var screen = await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenUrl == baseUrl);

            if (screen != null)
                return screen;

            // 2. جلب بالـ Controller/Action
            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(action))
            {
                var controllerAction = $"/{controller}/{action}";
                screen = await _context.Screens
                    .FirstOrDefaultAsync(s => s.ScreenUrl.Contains(controllerAction));
            }

            // 3. جلب بـ Wildcard (*)
            if (screen == null)
            {
                var screens = await _context.Screens
                    .Where(s => s.ScreenUrl.Contains("*"))
                    .ToListAsync();

                foreach (var s in screens)
                {
                    var pattern = s.ScreenUrl.Replace("*", "");
                    if (baseUrl.StartsWith(pattern))
                    {
                        screen = s;
                        break;
                    }
                }
            }

            return screen;
        }

        private string GetActionType(string action)
        {
            if (string.IsNullOrEmpty(action))
                return "View";

            var actionLower = action.ToLower();

            var createActions = new[] { "create", "add", "new", "insert", "register" };
            var editActions = new[] { "edit", "update", "modify", "change" };
            var deleteActions = new[] { "delete", "remove", "destroy" };
            var exportActions = new[] { "export", "download", "print" };

            if (createActions.Any(a => actionLower.Contains(a) || actionLower == a))
                return "Create";

            if (editActions.Any(a => actionLower.Contains(a) || actionLower == a))
                return "Edit";

            if (deleteActions.Any(a => actionLower.Contains(a) || actionLower == a))
                return "Delete";

            if (exportActions.Any(a => actionLower.Contains(a) || actionLower == a))
                return "Export";

            return "View";
        }
    }



}
