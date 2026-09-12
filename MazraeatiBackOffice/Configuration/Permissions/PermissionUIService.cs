using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration.Permissions
{
    public class PermissionUIService: IPermissionUIService
    {
        private readonly DataContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public PermissionUIService(DataContext context, IUnitOfWork UnitOfWork, IUserService userService)
        {
            _context = context;
            _unitOfWork = UnitOfWork;
            _userService = userService;
        }

        public async Task<bool> HasPermissionAsync(int userId, string screenUrl, string action)
        {
            //var screen = await _context.Screens
            //    .FirstOrDefaultAsync(s => s.ScreenUrl == screenUrl);
            var screen = await _unitOfWork.ScreenRepository.Table
                        .FirstOrDefaultAsync(s => s.ScreenUrl == screenUrl);
            if (screen == null)
                return false;

            //var permission = await _context.UserPermissions
            //    .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screen.Id);
            var permission = await _unitOfWork.UserPermissionRepository.Table
                            .FirstOrDefaultAsync(p => p.UserId == userId && p.ScreenId == screen.Id);

            if (permission == null)
                return false;

            return action.ToLower() switch
            {
                "view" => permission.CanView,
                "create" => permission.CanCreate,
                "edit" => permission.CanEdit,
                "delete" => permission.CanDelete,
                "export" => permission.CanExport,
                _ => false
            };
        }





        public async Task<bool> CanViewAsync(int userId, string screenUrl)
            => await HasPermissionAsync(userId, screenUrl, "view");

        public async Task<bool> CanCreateAsync(int userId, string screenUrl)
            => await HasPermissionAsync(userId, screenUrl, "create");

        public async Task<bool> CanEditAsync(int userId, string screenUrl)
            => await HasPermissionAsync(userId, screenUrl, "edit");

        public async Task<bool> CanDeleteAsync(int userId, string screenUrl)
            => await HasPermissionAsync(userId, screenUrl, "delete");

        public async Task<bool> CanExportAsync(int userId, string screenUrl)
            => await HasPermissionAsync(userId, screenUrl, "export");

        #region //New Methods ...

        public async Task<bool> HasPermissionAsync(string screenUrl, string action)
        {
            var principal = _userService.GetCurrentPrincipal();
            var userId = _userService.GetCurrentUserId(principal);

            if (userId == null)
                return false;

            return await HasPermissionAsync(userId.Value, screenUrl, action);
        }

        public async Task<bool> CanViewAsync(string screenUrl)
            => await HasPermissionAsync(screenUrl, "view");

        public async Task<bool> CanCreateAsync(string screenUrl)
            => await HasPermissionAsync(screenUrl, "create");

        public async Task<bool> CanEditAsync(string screenUrl)
            => await HasPermissionAsync(screenUrl, "edit");

        public async Task<bool> CanDeleteAsync(string screenUrl)
            => await HasPermissionAsync(screenUrl, "delete");

        public async Task<bool> CanExportAsync(string screenUrl)
            => await HasPermissionAsync(screenUrl, "export");

        #endregion
       private ClaimsPrincipal GetCurrentPrincipal()
        {
            return _userService.GetCurrentPrincipal();
        }



    }

}
