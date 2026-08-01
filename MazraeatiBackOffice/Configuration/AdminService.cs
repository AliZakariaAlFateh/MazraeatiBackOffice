using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class AdminService : IAdminService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // ==================== Auth Methods ====================

        public async Task<AdminUser> LoginAsync(string username, string password)
        {
            var user = await _context.AdminUsers
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == username && u.IsActive);

            if (user == null)
                return null;
                
            // TODO: Use BCrypt for password hashing
            if (user.Password != password)
                return null;

            return user;
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // ==================== User Management ====================

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new AdminUser
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password, // TODO: Hash this
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                await _context.AdminUsers.AddAsync(user);
                await _context.SaveChangesAsync();

                if (model.RoleIds != null && model.RoleIds.Any())
                {
                    foreach (var roleId in model.RoleIds)
                    {
                        var userRole = new UserRole
                        {
                            UserId = user.Id,
                            RoleId = roleId
                        };
                        await _context.UserRoles.AddAsync(userRole);
                    }
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(EditUserModel model)
        {
            var user = await _context.AdminUsers
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.IsActive = model.IsActive;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = model.Password; // TODO: Hash this
                }

                _context.AdminUsers.Update(user);

                // Update roles
                var existingRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id);
                _context.UserRoles.RemoveRange(existingRoles);

                if (model.RoleIds != null && model.RoleIds.Any())
                {
                    foreach (var roleId in model.RoleIds)
                    {
                        _context.UserRoles.Add(new UserRole
                        {
                            UserId = user.Id,
                            RoleId = roleId
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        //public async Task<bool> DeleteUserAsync(int userId)
        //{
        //    var user = await _context.AdminUsers.FindAsync(userId);
        //    if (user == null) return false;

        //    _context.AdminUsers.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> DeleteUserAsync(int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. جلب المستخدم مع الـ UserRoles بتاعته
                var user = await _context.AdminUsers
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return false;

                // 2. حذف الـ UserRoles المرتبطة أولاً
                if (user.UserRoles != null && user.UserRoles.Any())
                {
                    _context.UserRoles.RemoveRange(user.UserRoles);
                }

                // 3. بعد كده حذف المستخدم
                _context.AdminUsers.Remove(user);

                // 4. حفظ التغييرات
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }

        public async Task<bool> ToggleUserStatusAsync(int userId)
        {
            var user = await _context.AdminUsers.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = !user.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AdminUser> GetUserByIdAsync(int userId)
        {
            return await _context.AdminUsers
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _context.AdminUsers
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();

            return users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FullName,
                Email = u.Email,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate,
                Roles = u.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>()
            }).ToList();
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }

        // ==================== Role Management ====================

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<bool> CreateRoleAsync(string roleName, string description)
        {
            if (await _context.Roles.AnyAsync(r => r.Name == roleName))
                return false;

            var role = new Role
            {
                Name = roleName,
                Description = description
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoleAsync(int roleId, string roleName, string description)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            role.Name = roleName;
            role.Description = description;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        // ==================== Helper Methods ====================

        public async Task<bool> IsUsernameExistsAsync(string username, int? excludeUserId = null)
        {
            return await _context.AdminUsers
                .AnyAsync(u => u.UserName == username && (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null)
        {
            if (string.IsNullOrEmpty(email)) return false;

            return await _context.AdminUsers
                .AnyAsync(u => u.Email == email && (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
        }

        public async Task<AdminUser> GetUserByUsernameAsync(string username)
        {
            return await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> UpdateIsSuperAdminAsync(int userId, bool isSuperAdmin)
        {
            var user = await _context.AdminUsers.FindAsync(userId);
            if (user == null) return false;

            user.IsSuperAdmin = isSuperAdmin;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
