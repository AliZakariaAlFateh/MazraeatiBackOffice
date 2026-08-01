using MazraeatiBackOffice.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public interface IUserService
    {
        Task<AdminUser> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<List<string>> GetUserRolesAsync(int userId);
        bool IsAuthenticated(ClaimsPrincipal principal);
        int? GetCurrentUserId(ClaimsPrincipal principal);
        Task<bool> IsSuperAdminAsync(int userId);
    }
}
