using Google;
using MazraeatiBackOffice.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AdminUser> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            var userId = GetCurrentUserId(principal);
            if (userId == null) return null;

            return await _context.AdminUsers
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            return roles;
        }

        public bool IsAuthenticated(ClaimsPrincipal principal)
        {
            return principal?.Identity?.IsAuthenticated == true;
        }

        public int? GetCurrentUserId(ClaimsPrincipal principal)
        {
            var userIdClaim = principal?.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            return null;
        }

        public async Task<bool> IsSuperAdminAsync(int userId)
        {
            var user = await _context.AdminUsers.FindAsync(userId);
            return user?.IsSuperAdmin == true;
        }
    }
}
