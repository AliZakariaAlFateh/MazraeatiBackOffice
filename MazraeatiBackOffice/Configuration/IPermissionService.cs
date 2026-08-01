using MazraeatiBackOffice.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public interface IPermissionService
    {
        Task<List<Screen>> GetUserScreensAsync(ClaimsPrincipal user);
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string screenUrl, string action);
        Task<UserPermission> GetUserPermissionAsync(int userId, int screenId);
        Task UpdateUserPermissionAsync(UserPermission permission);
        Task<bool> IsSuperAdminAsync(ClaimsPrincipal user);
        //int? GetCurrentUserId(ClaimsPrincipal user);
    }
}
