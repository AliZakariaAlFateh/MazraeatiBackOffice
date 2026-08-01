using MazraeatiBackOffice.Core;
using MazraeatiBackOffice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public interface IAdminService
    {
        // Auth
        Task<AdminUser> LoginAsync(string username, string password);
        Task LogoutAsync();

        // User Management
        Task<bool> RegisterAsync(RegisterModel model);
        Task<bool> UpdateUserAsync(EditUserModel model);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> ToggleUserStatusAsync(int userId);
        Task<AdminUser> GetUserByIdAsync(int userId);
        Task<List<UserViewModel>> GetAllUsersAsync();

        // Role Management
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<bool> CreateRoleAsync(string roleName, string description);
        Task<bool> UpdateRoleAsync(int roleId, string roleName, string description);
        Task<bool> DeleteRoleAsync(int roleId);

        // Helper Methods
        Task<bool> IsUsernameExistsAsync(string username, int? excludeUserId = null);
        Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null);
        Task<List<string>> GetUserRolesAsync(int userId);

        Task<AdminUser> GetUserByUsernameAsync(string username);
        Task<bool> UpdateIsSuperAdminAsync(int userId, bool isSuperAdmin);
    }
}
