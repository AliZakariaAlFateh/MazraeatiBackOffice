using Google;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MazraeatiBackOffice.Models;
using System.Text.Json;

namespace MazraeatiBackOffice.Controllers
{
    [Authorize]
    public class ScreensController : BaseController
    {


        private readonly DataContext _context;

        public ScreensController(DataContext context)
        {
            _context = context;
        }

        // =============================================
        // 1. عرض قائمة الشاشات
        // =============================================
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var screens = await _context.Screens
                .Include(s => s.Parent)
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new ScreenViewModel
                {
                    Id = s.Id,
                    ScreenName = s.ScreenName,
                    ScreenUrl = s.ScreenUrl,
                    Icon = s.Icon,
                    DisplayOrder = s.DisplayOrder,
                    ParentId = s.ParentId,
                    ParentName = s.Parent.ScreenName,
                    IsActive = s.IsActive,
                    IsMenu = s.IsMenu,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            var viewModel = new ScreenManagementViewModel
            {
                Screens = screens,
                NewScreen = new Screen()
            };

            return View(viewModel);
        }

        // =============================================
        // 2. إضافة شاشة جديدة (POST)
        // =============================================
        [HttpPost]
        public async Task<IActionResult> AddScreen([FromBody] Screen newScreen)
        {
            try
            {
                if (string.IsNullOrEmpty(newScreen.ScreenName) || string.IsNullOrEmpty(newScreen.ScreenUrl))
                {
                    return Json(new { success = false, message = "اسم الصفحة والـ URL مطلوبان" });
                }

                // التحقق من وجود URL مكرر
                var exists = await _context.Screens
                    .AnyAsync(s => s.ScreenUrl == newScreen.ScreenUrl);

                if (exists)
                {
                    return Json(new { success = false, message = "هذا الرابط مستخدم من قبل" });
                }

                newScreen.CreatedAt = DateTime.Now;
                newScreen.UpdatedAt = DateTime.Now;
                newScreen.IsActive = true;

                _context.Screens.Add(newScreen);
                await _context.SaveChangesAsync();

                // إضافة صلاحيات للـ SuperAdmin على الصفحة الجديدة
                var superAdmins = await _context.AdminUsers
                    .Where(u => u.IsSuperAdmin && u.IsActive)
                    .ToListAsync();

                foreach (var admin in superAdmins)
                {
                    var permission = new UserPermission
                    {
                        UserId = admin.Id,
                        ScreenId = newScreen.Id,
                        CanView = true,
                        CanCreate = true,
                        CanEdit = true,
                        CanDelete = true,
                        CanExport = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.UserPermissions.Add(permission);
                }

                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "تم إضافة الصفحة بنجاح",
                    screenId = newScreen.Id,
                    screen = new
                    {
                        newScreen.Id,
                        newScreen.ScreenName,
                        newScreen.ScreenUrl,
                        newScreen.Icon,
                        newScreen.DisplayOrder,
                        newScreen.ParentId,
                        newScreen.IsMenu,
                        newScreen.IsActive,
                        newScreen.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // =============================================
        // 3. تحديث شاشة (POST)
        // =============================================
        [HttpPost]
        public async Task<IActionResult> UpdateScreen([FromBody] Screen updatedScreen)
        {
            try
            {
                var screen = await _context.Screens.FindAsync(updatedScreen.Id);
                if (screen == null)
                    return Json(new { success = false, message = "الصفحة غير موجودة" });

                // التحقق من وجود URL مكرر (باستثناء نفس الشاشة)
                var exists = await _context.Screens
                    .AnyAsync(s => s.ScreenUrl == updatedScreen.ScreenUrl && s.Id != updatedScreen.Id);

                if (exists)
                {
                    return Json(new { success = false, message = "هذا الرابط مستخدم من قبل" });
                }

                screen.ScreenName = updatedScreen.ScreenName;
                screen.ScreenUrl = updatedScreen.ScreenUrl;
                screen.Icon = updatedScreen.Icon;
                screen.DisplayOrder = updatedScreen.DisplayOrder;
                screen.ParentId = updatedScreen.ParentId;
                screen.IsMenu = updatedScreen.IsMenu;
                screen.IsActive = updatedScreen.IsActive;
                screen.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم تحديث الصفحة بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // =============================================
        // 4. حذف شاشة (POST)
        // =============================================
        [HttpPost]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            try
            {
                var screen = await _context.Screens
                    .Include(s => s.SubScreens)
                    .Include(s => s.UserPermissions)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (screen == null)
                    return Json(new { success = false, message = "الصفحة غير موجودة" });

                // التحقق من وجود صفحات تابعة
                if (screen.SubScreens != null && screen.SubScreens.Any())
                {
                    return Json(new { success = false, message = "لا يمكن حذف الصفحة لأنها تحتوي على صفحات تابعة" });
                }

                // حذف الصلاحيات المرتبطة
                if (screen.UserPermissions != null && screen.UserPermissions.Any())
                {
                    _context.UserPermissions.RemoveRange(screen.UserPermissions);
                }

                // حذف الصفحة
                _context.Screens.Remove(screen);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حذف الصفحة بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // =============================================
        // 5. جلب بيانات شاشة للتعديل (GET)
        // =============================================
        public async Task<IActionResult> GetScreen(int id)
        {
            var screen = await _context.Screens
                .Select(s => new
                {
                    s.Id,
                    s.ScreenName,
                    s.ScreenUrl,
                    s.Icon,
                    s.DisplayOrder,
                    s.ParentId,
                    s.IsMenu,
                    s.IsActive
                })
                .FirstOrDefaultAsync(s => s.Id == id);

            if (screen == null)
                return Json(new { success = false, message = "الصفحة غير موجودة" });

            return Json(new { success = true, screen });
        }

        // =============================================
        // 6. التحقق من وجود URL مكرر
        // =============================================
        [HttpPost]
        public async Task<IActionResult> CheckUrl(string url, int? excludeId = null)
        {
            var exists = await _context.Screens
                .AnyAsync(s => s.ScreenUrl == url && (excludeId == null || s.Id != excludeId));

            return Json(new { exists = exists });
        }

        // =============================================
        // 7. جلب الصفحات المتاحة كـ Parent
        // =============================================
        public async Task<IActionResult> GetParentScreens(int? excludeId = null)
        {
            var screens = await _context.Screens
                .Where(s => s.IsActive && (excludeId == null || s.Id != excludeId))
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new { s.Id, s.ScreenName })
                .ToListAsync();

            return Json(screens);
        }

        // =============================================
        // 8. تبديل حالة الشاشة (تفعيل/تعطيل)
        // =============================================
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null)
                return Json(new { success = false, message = "الصفحة غير موجودة" });

            screen.IsActive = !screen.IsActive;
            screen.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = $"تم {(screen.IsActive ? "تفعيل" : "تعطيل")} الصفحة بنجاح",
                isActive = screen.IsActive
            });
        }


        //private readonly DataContext _context;
        //private readonly IPermissionService _permissionService;
        //private readonly IUserService _userService;

        //public ScreensController(DataContext context, IPermissionService permissionService, IUserService userService)
        //{
        //    _context = context;
        //    _permissionService = permissionService;
        //    _userService= userService;
        //}

        //// عرض كل الصفحات مع صلاحيات المستخدم الحالي (SuperAdmin بس)
        //public async Task<IActionResult> Index()
        //{
        //    // جلب كل الشاشات
        //    var screens = await _context.Screens
        //        .Include(s => s.Parent)
        //        .OrderBy(s => s.DisplayOrder)
        //        .ToListAsync();

        //    // جلب المستخدم الحالي
        //    var userId = _userService.GetCurrentUserId(User);
        //    var currentUser = await _context.AdminUsers.FindAsync(userId);

        //    // جلب صلاحيات المستخدم الحالي لكل شاشة (لو هو SuperAdmin)
        //    var viewModel = new ScreenManagementViewModel
        //    {
        //        Screens = new List<ScreenPermissionViewModel>(),
        //        NewScreen = new Screen()
        //    };

        //    foreach (var screen in screens)
        //    {
        //        var userPermission = await _permissionService.GetUserPermissionAsync(userId.Value, screen.Id);

        //        viewModel.Screens.Add(new ScreenPermissionViewModel
        //        {
        //            ScreenId = screen.Id,
        //            ScreenName = screen.ScreenName,
        //            ScreenUrl = screen.ScreenUrl,
        //            Icon = screen.Icon,
        //            DisplayOrder = screen.DisplayOrder,
        //            ParentId = screen.ParentId,
        //            ParentName = screen.Parent?.ScreenName,
        //            IsActive = screen.IsActive,
        //            IsMenu = screen.IsMenu,
        //            CanView = userPermission.CanView,
        //            CanCreate = userPermission.CanCreate,
        //            CanEdit = userPermission.CanEdit,
        //            CanDelete = userPermission.CanDelete,
        //            CanExport = userPermission.CanExport
        //        });
        //    }

        //    return View(viewModel);
        //}

        //// إضافة صفحة جديدة
        //[HttpPost]
        //public async Task<IActionResult> AddScreen(Screen newScreen)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        newScreen.CreatedAt = DateTime.Now;
        //        newScreen.UpdatedAt = DateTime.Now;
        //        newScreen.IsActive = true;

        //        _context.Screens.Add(newScreen);
        //        await _context.SaveChangesAsync();

        //        // إضافة صلاحيات للـ SuperAdmin على الصفحة الجديدة
        //        var superAdmins = await _context.AdminUsers
        //            .Where(u => u.IsSuperAdmin && u.IsActive)
        //            .ToListAsync();

        //        foreach (var admin in superAdmins)
        //        {
        //            var permission = new UserPermission
        //            {
        //                UserId = admin.Id,
        //                ScreenId = newScreen.Id,
        //                CanView = true,
        //                CanCreate = true,
        //                CanEdit = true,
        //                CanDelete = true,
        //                CanExport = true,
        //                CreatedAt = DateTime.Now,
        //                UpdatedAt = DateTime.Now
        //            };
        //            _context.UserPermissions.Add(permission);
        //        }

        //        await _context.SaveChangesAsync();

        //        return Json(new { success = true, message = "تم إضافة الصفحة بنجاح" });
        //    }

        //    return Json(new { success = false, message = "حدث خطأ في إضافة الصفحة" });
        //}

        //// تحديث صلاحية واحدة (تحديث فوري)
        //[HttpPost]
        //public async Task<IActionResult> UpdatePermission(int screenId, string permissionType, bool value)
        //{
        //    var userId = _userService.GetCurrentUserId(User);

        //    if (userId == null)
        //        return Json(new { success = false, message = "المستخدم غير مسجل" });

        //    // جلب صلاحية المستخدم لهذه الشاشة
        //    var permission = await _context.UserPermissions
        //        .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screenId);

        //    if (permission == null)
        //    {
        //        permission = new UserPermission
        //        {
        //            UserId = userId.Value,
        //            ScreenId = screenId,
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = DateTime.Now
        //        };
        //        _context.UserPermissions.Add(permission);
        //    }

        //    // تحديث الصلاحية المطلوبة
        //    switch (permissionType.ToLower())
        //    {
        //        case "view":
        //            permission.CanView = value;
        //            break;
        //        case "create":
        //            permission.CanCreate = value;
        //            break;
        //        case "edit":
        //            permission.CanEdit = value;
        //            break;
        //        case "delete":
        //            permission.CanDelete = value;
        //            break;
        //        case "export":
        //            permission.CanExport = value;
        //            break;
        //        default:
        //            return Json(new { success = false, message = "نوع صلاحية غير صحيح" });
        //    }

        //    permission.UpdatedAt = DateTime.Now;
        //    await _context.SaveChangesAsync();

        //    return Json(new { success = true, message = "تم تحديث الصلاحية بنجاح" });
        //}

        //// حفظ كل الصلاحيات (Save All)
        //[HttpPost]
        //public async Task<IActionResult> SaveAllPermissions([FromBody] List<ScreenPermissionViewModel> permissions)
        //{
        //    var userId = _userService.GetCurrentUserId(User);

        //    if (userId == null)
        //        return Json(new { success = false, message = "المستخدم غير مسجل" });

        //    foreach (var perm in permissions)
        //    {
        //        var permission = await _context.UserPermissions
        //            .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == perm.ScreenId);

        //        if (permission == null)
        //        {
        //            permission = new UserPermission
        //            {
        //                UserId = userId.Value,
        //                ScreenId = perm.ScreenId,
        //                CreatedAt = DateTime.Now,
        //                UpdatedAt = DateTime.Now
        //            };
        //            _context.UserPermissions.Add(permission);
        //        }

        //        permission.CanView = perm.CanView;
        //        permission.CanCreate = perm.CanCreate;
        //        permission.CanEdit = perm.CanEdit;
        //        permission.CanDelete = perm.CanDelete;
        //        permission.CanExport = perm.CanExport;
        //        permission.UpdatedAt = DateTime.Now;
        //    }

        //    await _context.SaveChangesAsync();

        //    return Json(new { success = true, message = "تم حفظ جميع الصلاحيات بنجاح" });
        //}

        //// حذف صفحة
        //[HttpPost]
        //public async Task<IActionResult> DeleteScreen(int id)
        //{
        //    var screen = await _context.Screens.FindAsync(id);
        //    if (screen == null)
        //        return Json(new { success = false, message = "الصفحة غير موجودة" });

        //    // حذف الصلاحيات المرتبطة
        //    var permissions = await _context.UserPermissions
        //        .Where(p => p.ScreenId == id)
        //        .ToListAsync();
        //    _context.UserPermissions.RemoveRange(permissions);

        //    // حذف الصفحة
        //    _context.Screens.Remove(screen);
        //    await _context.SaveChangesAsync();

        //    return Json(new { success = true, message = "تم حذف الصفحة بنجاح" });
        //}

        //// جلب الصفحات المتاحة كـ Parent
        //public async Task<IActionResult> GetParentScreens()
        //{
        //    var screens = await _context.Screens
        //        .Where(s => s.IsActive)
        //        .OrderBy(s => s.DisplayOrder)
        //        .Select(s => new { s.Id, s.ScreenName })
        //        .ToListAsync();

        //    return Json(screens);
        //}

        //// التحقق من وجود URL مكرر
        //[HttpPost]
        //public async Task<IActionResult> CheckUrl(string url)
        //{
        //    var exists = await _context.Screens
        //        .AnyAsync(s => s.ScreenUrl == url);

        //    return Json(new { exists = exists });
        //}
    }
}
