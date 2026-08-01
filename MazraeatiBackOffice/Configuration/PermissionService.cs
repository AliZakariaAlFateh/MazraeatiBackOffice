using Google;
using MazraeatiBackOffice.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class PermissionService : IPermissionService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public PermissionService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<List<Screen>> GetUserScreensAsync(ClaimsPrincipal principal)
        {
            if (!_userService.IsAuthenticated(principal))
                return new List<Screen>();

            var userId = _userService.GetCurrentUserId(principal);
            if (userId == null) return new List<Screen>();

            // Check if user is SuperAdmin (has full access)
            var isSuperAdmin = await IsSuperAdminAsync(principal);

            if (isSuperAdmin)
            {
                // Return all active screens that are menus
                return await _context.Screens
                    .Where(s => s.IsActive && s.IsMenu)
                    .OrderBy(s => s.DisplayOrder)
                    .Include(s => s.SubScreens.Where(sub => sub.IsActive))
                    .ToListAsync();
            }

            // For regular users, get screens from UserPermissions
            var screenIds = await _context.UserPermissions
                .Where(p => p.UserId == userId && p.CanView)
                .Select(p => p.ScreenId)
                .Distinct()
                .ToListAsync();

            var screens = await _context.Screens
                .Where(s => screenIds.Contains(s.Id) && s.IsActive && s.IsMenu)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            foreach (var screen in screens)
            {
                screen.SubScreens = await _context.Screens
                    .Where(s => s.ParentId == screen.Id && s.IsActive && s.IsMenu)
                    .OrderBy(s => s.DisplayOrder)
                    .ToListAsync();
            }

            return screens;
        }

        public async Task<bool> HasPermissionAsync(ClaimsPrincipal principal, string screenUrl, string action)
        {
            if (!_userService.IsAuthenticated(principal))
                return false;

            var userId = _userService.GetCurrentUserId(principal);
            if (userId == null) return false;

            // SuperAdmin has all permissions
            if (await IsSuperAdminAsync(principal))
                return true;

            // Get the screen
            var screen = await GetScreenByUrlAsync(screenUrl);
            if (screen == null)
                return true; // Screen not registered, allow access

            // Get user permission for this screen
            var permission = await _context.UserPermissions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screen.Id);

            if (permission == null)
                return false;

            // Check based on action type
            return action switch
            {
                "View" => permission.CanView,
                "Create" => permission.CanCreate,
                "Edit" => permission.CanEdit,
                "Delete" => permission.CanDelete,
                "Approve" => permission.CanApprove,
                "Export" => permission.CanExport,
                _ => false
            };
        }

        public async Task<UserPermission> GetUserPermissionAsync(int userId, int screenId)
        {
            var permission = await _context.UserPermissions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screenId);

            if (permission == null)
            {
                permission = new UserPermission
                {
                    UserId = userId,
                    ScreenId = screenId,
                    CanView = false,
                    CanCreate = false,
                    CanEdit = false,
                    CanDelete = false,
                    CanApprove = false,
                    CanExport = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }

            return permission;
        }

        public async Task UpdateUserPermissionAsync(UserPermission permission)
        {
            permission.UpdatedAt = DateTime.Now;

            var existing = await _context.UserPermissions
                .FirstOrDefaultAsync(p => p.UserId == permission.UserId && p.ScreenId == permission.ScreenId);

            if (existing == null)
            {
                _context.UserPermissions.Add(permission);
            }
            else
            {
                existing.CanView = permission.CanView;
                existing.CanCreate = permission.CanCreate;
                existing.CanEdit = permission.CanEdit;
                existing.CanDelete = permission.CanDelete;
                existing.CanApprove = permission.CanApprove;
                existing.CanExport = permission.CanExport;
                existing.UpdatedAt = permission.UpdatedAt;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSuperAdminAsync(ClaimsPrincipal principal)
        {
            var userId = _userService.GetCurrentUserId(principal);
            if (userId == null) return false;

            var user = await _context.AdminUsers.FindAsync(userId);
            return user?.IsSuperAdmin == true;
        }

        private async Task<Screen> GetScreenByUrlAsync(string url)
        {
            // Remove query string if any
            var baseUrl = url.Split('?')[0];

            // Try to match exact URL or pattern with wildcard
            var screen = await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenUrl == baseUrl);

            if (screen == null)
            {
                // Check for wildcard patterns (like /Users/Edit/*)
                screen = await _context.Screens
                    .FirstOrDefaultAsync(s => baseUrl.StartsWith(s.ScreenUrl.Replace("*", "")));
            }

            return screen;
        }

        //public int? GetCurrentUserId(ClaimsPrincipal principal)
        //{
        //    // جلب الـ UserId من الـ Claims
        //    var userIdClaim = principal?.FindFirst("UserId")?.Value;

        //    if (int.TryParse(userIdClaim, out int userId))
        //        return userId;

        //    return null;
        //}

    }
}
