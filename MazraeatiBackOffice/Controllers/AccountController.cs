using Google;
using MazraeatiBackOffice.Configuration;
using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Dto;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Controllers
{
    public class AccountController : BaseController
    {
        public enum NotifyType
        {
            Success,
            Error
        }
        private readonly DataContext _context;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        private readonly IAdminService _adminService;
        public AccountController(IAdminService adminService, DataContext context,
            IPermissionService permissionService,
            IUserService userService)
        {
            _adminService = adminService;
            _context = context;
            _permissionService = permissionService;
            _userService = userService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var users = await _adminService.GetAllUsersAsync();
            return View(users);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = await _adminService.LoginAsync(model.UserName, model.Password);

                if (user != null)
                {
                    var roles = await _adminService.GetUserRolesAsync(user.Id);

                    var claims = new List<Claim>
            {
                // ===== مهم: استخدم نفس الاسم اللي الـ Filter بيدور عليه =====
                new Claim("UserId", user.Id.ToString()),           // ✅ نفس الاسم
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    DateTime expiresUtc;
                    TimeSpan sessionTimeout;

                    if (model.RememberMe)
                    {
                        expiresUtc = DateTime.UtcNow.AddDays(30);
                        sessionTimeout = TimeSpan.FromDays(30);
                    }
                    else
                    {
                        expiresUtc = DateTime.UtcNow.AddHours(24);
                        sessionTimeout = TimeSpan.FromHours(24);
                    }

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = expiresUtc,
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Session
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("Email", user.Email ?? "");
                    HttpContext.Session.SetInt32("IsActive", user.IsActive ? 1 : 0);
                    HttpContext.Session.SetInt32("SessionTimeout", (int)sessionTimeout.TotalMinutes);

                    var sessionUser = new AdminUserSessionDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Email = user.Email ?? "",
                        IsActive = user.IsActive
                    };

                    var userJson = JsonSerializer.Serialize(sessionUser);
                    HttpContext.Session.SetString("AdminUser", userJson);

                    TempData["Success"] = $"Welcome back, {user.FullName}!";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid username or password");
            }

            return View(model);
        }



        #region  Register
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Register()
        {
            var roles = await _adminService.GetAllRolesAsync();

            var model = new RegisterModel
            {
                AvailableRoles = roles.Select(r => new RoleCheckboxModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsChecked = false
                }).ToList()
            };

            return View(model);
        }

        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var roles = await _adminService.GetAllRolesAsync();
                model.AvailableRoles = roles.Select(r => new RoleCheckboxModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsChecked = model.RoleIds?.Contains(r.Id) ?? false
                }).ToList();

                // Check if username exists
                if (await _adminService.IsUsernameExistsAsync(model.UserName))
                {
                    ModelState.AddModelError("UserName", "Username already exists");
                    return View(model);
                }

                // Check if email exists
                if (!string.IsNullOrEmpty(model.Email) && await _adminService.IsEmailExistsAsync(model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                var result = await _adminService.RegisterAsync(model);

                if (result)
                {
                    //بعد اضافة السجل بنجاح
                    //هنا عايز أقول لو ال roles 
                    //تحتوى على r.id=1
                    // عايز أعمل تحديث لجدول الAdminUser with the User currently saved
                    //I want make update on column "IsSuperAdmin=1"

                    //write Code Here ...


                    //TempData["Success"] = System.Web.HttpUtility.HtmlEncode($" تم إضافة بيانات {model.UserName} بنجاح!");
                    //SuccessNotification("تم اضافة السجل بنجاح");
                    //return RedirectToAction(nameof(Index));
                    // ========== بعد إضافة السجل بنجاح ==========
                    // جلب المستخدم الذي تم إضافته
                    var newUser = await _adminService.GetUserByUsernameAsync(model.UserName);

                    if (newUser != null)
                    {
                        // التحقق إذا كان RoleId = 1 (SuperAdmin)
                        bool isSuperAdmin = model.RoleIds != null && model.RoleIds.Contains(1);
                        await _adminService.UpdateIsSuperAdminAsync(newUser.Id, isSuperAdmin);
                    }

                    TempData["Success"] = System.Web.HttpUtility.HtmlEncode($" تم إضافة بيانات {model.UserName} بنجاح!");
                    SuccessNotification("تم اضافة السجل بنجاح");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Error creating user");

            }



            return View(model);
        }
        #endregion


        #region Edit Active& Deactive Status and Delete User
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = await _adminService.GetAllRolesAsync();
            var userRoleIds = user.UserRoles?.Select(ur => ur.RoleId).ToList() ?? new List<int>();

            var model = new EditUserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Password=user.Password,
                IsActive = user.IsActive,
                RoleIds = userRoleIds,
                AvailableRoles = allRoles.Select(r => new RoleCheckboxModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsChecked = userRoleIds.Contains(r.Id)
                }).ToList()
            };

            return View(model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(int id, EditUserModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Check if username exists for another user
                if (await _adminService.IsUsernameExistsAsync(model.UserName, model.Id))
                {
                    ModelState.AddModelError("UserName", "Username already exists");
                    return View(model);
                }

                // Check if email exists for another user
                if (!string.IsNullOrEmpty(model.Email) && await _adminService.IsEmailExistsAsync(model.Email, model.Id))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(model);
                }

                var result = await _adminService.UpdateUserAsync(model);

                if (result)
                {

                    TempData["Success"] = System.Web.HttpUtility.HtmlEncode($" تم تحديث بيانات {model.UserName} بنجاح!");
                    SuccessNotification("تم تحديث السجل بنجاح");
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Error updating user");
            }

            var allRoles = await _adminService.GetAllRolesAsync();
            model.AvailableRoles = allRoles.Select(r => new RoleCheckboxModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                IsChecked = model.RoleIds?.Contains(r.Id) ?? false
            }).ToList();

            return View(model);
        }

        // POST: Users/ToggleStatus/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        //public async Task<IActionResult> ToggleStatus(int id)
        //{
        //    var result = await _adminService.ToggleUserStatusAsync(id);

        //    if (result)
        //    {
        //        TempData["Success"] = "User status updated successfully!";
        //        SuccessNotification("تم التحديث");
        //    }
        //    else
        //    {
        //        TempData["Error"] = "Error updating user status";
        //        ErrorNotification("حدث خطأ أثناء التحديث");
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var result = await _adminService.ToggleUserStatusAsync(id);

                if (result)
                {
                    var user = await _adminService.GetUserByIdAsync(id);
                    return Json(new { success = true, isActive = user.IsActive, message = "تم تغيير حالة المستخدم بنجاح" });
                }

                return Json(new { success = false, message = "فشل تغيير حالة المستخدم" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Users/Delete/5
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var result = await _adminService.DeleteUserAsync(id);

        //    if (result)
        //    {
        //        TempData["Success"] = "User deleted successfully!";
        //    }
        //    else
        //    {
        //        TempData["Error"] = "Error deleting user";
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _adminService.DeleteUserAsync(id);

                if (result)
                {
                    return Json(new { success = true, message = "تم حذف المستخدم بنجاح" });
                }

                return Json(new { success = false, message = "فشل حذف المستخدم" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion




        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        //public IActionResult AccessDenied(string ReturnUrl = "")
        //{
        //    return RedirectToAction("Login");
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // جلب معلومات المستخدم الحالي (لو فيه)
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.FindFirst("FullName")?.Value ?? User.Identity.Name;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
                ViewBag.UserName = userName;
                ViewBag.UserRole = userRole;
            }

            return View();
        }


        // =============================================
        // 2. صفحة إدارة صلاحيات مستخدم معين
        // =============================================
        public async Task<IActionResult> ManagePermissions(int id)
        {
            var user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound("المستخدم غير موجود");
            }

            // جلب كل الشاشات النشطة
            var allScreens = await _context.Screens
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            // جلب صلاحيات المستخدم لكل شاشة
            var permissions = new List<UserPermissionViewModel>();

            foreach (var screen in allScreens)
            {
                var userPermission = await _permissionService.GetUserPermissionAsync(id, screen.Id);

                permissions.Add(new UserPermissionViewModel
                {
                    ScreenId = screen.Id,
                    ScreenName = screen.ScreenName,
                    ScreenUrl = screen.ScreenUrl,
                    Icon = screen.Icon,
                    ParentId = screen.ParentId,
                    DisplayOrder = screen.DisplayOrder,
                    IsMenu = screen.IsMenu,
                    CanView = userPermission.CanView,
                    CanCreate = userPermission.CanCreate,
                    CanEdit = userPermission.CanEdit,
                    CanDelete = userPermission.CanDelete,
                    CanExport = userPermission.CanExport
                });
            }

            var viewModel = new ManageUserPermissionsViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                IsSuperAdmin = user.IsSuperAdmin,
                Permissions = permissions
            };

            return View(viewModel);
        }

        // =============================================
        // 3. تحديث صلاحية واحدة (حفظ فوري)
        // =============================================
        [HttpPost]
        public async Task<IActionResult> UpdatePermission(int userId, int screenId, string permissionType, bool value)
        {
            LogFile logFile = new LogFile();

            try
            {
                var permission = await _context.UserPermissions
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screenId);

                if (permission == null)
                {
                    permission = new UserPermission
                    {
                        UserId = userId,
                        ScreenId = screenId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _context.UserPermissions.Add(permission);
                }

                switch (permissionType.ToLower())
                {
                    case "view":
                        permission.CanView = value;
                        break;
                    case "create":
                        permission.CanCreate = value;
                        break;
                    case "edit":
                        permission.CanEdit = value;
                        break;
                    case "delete":
                        permission.CanDelete = value;
                        break;
                    case "export":
                        permission.CanExport = value;
                        break;
                    default:
                        return Json(new { success = false, message = "نوع صلاحية غير صحيح" });
                }

                permission.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم تحديث الصلاحية بنجاح" });
            }
            catch (Exception ex)
            {
                //note : the folder log name is ==>> FarmerCrashLog
                ErrorNotification($"Error while UpdatePermission : {ex.Message}. Please contact the administrator.");
                logFile.LogCustomInfo("UpdatePermission Account - Exception Message ", ex.Message);
                logFile.LogCustomInfo("UpdatePermission Account - Stack Trace Message ", ex.StackTrace);
                logFile.LogCustomInfo("UpdatePermission Account - Inner Exception Message ", ex.InnerException.ToString());
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // =============================================
        // 4. حفظ جميع الصلاحيات دفعة واحدة
        // =============================================
        [HttpPost]
        public async Task<IActionResult> SaveAllPermissions(int userId, [FromBody] List<UserPermissionViewModel> permissions)
        {
            try
            {
                foreach (var perm in permissions)
                {
                    var permission = await _context.UserPermissions
                        .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == perm.ScreenId);

                    if (permission == null)
                    {
                        permission = new UserPermission
                        {
                            UserId = userId,
                            ScreenId = perm.ScreenId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _context.UserPermissions.Add(permission);
                    }

                    permission.CanView = perm.CanView;
                    permission.CanCreate = perm.CanCreate;
                    permission.CanEdit = perm.CanEdit;
                    permission.CanDelete = perm.CanDelete;
                    permission.CanExport = perm.CanExport;
                    permission.UpdatedAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حفظ جميع الصلاحيات بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // =============================================
        // 5. تبديل حالة المستخدم (تفعيل/تعطيل)
        // =============================================
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ToggleStatus(int id)
        //{
        //    var user = await _context.AdminUsers.FindAsync(id);
        //    if (user == null)
        //        return Json(new { success = false, message = "المستخدم غير موجود" });

        //    user.IsActive = !user.IsActive;
        //    await _context.SaveChangesAsync();

        //    return Json(new
        //    {
        //        success = true,
        //        message = $"تم {(user.IsActive ? "تفعيل" : "تعطيل")} المستخدم بنجاح",
        //        isActive = user.IsActive
        //    });
        //}

        // =============================================
        // 6. حذف المستخدم
        // =============================================
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.AdminUsers
                .Include(u => u.UserRoles)
                .Include(u => u.UserPermissions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return Json(new { success = false, message = "المستخدم غير موجود" });

            // منع حذف الـ SuperAdmin الوحيد
            if (user.IsSuperAdmin)
            {
                var superAdminCount = await _context.AdminUsers
                    .CountAsync(u => u.IsSuperAdmin && u.IsActive);

                if (superAdminCount <= 1)
                    return Json(new { success = false, message = "لا يمكن حذف الـ SuperAdmin الوحيد في النظام" });
            }

            _context.UserPermissions.RemoveRange(user.UserPermissions);
            _context.UserRoles.RemoveRange(user.UserRoles);
            _context.AdminUsers.Remove(user);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "تم حذف المستخدم بنجاح" });
        }



    }
}
