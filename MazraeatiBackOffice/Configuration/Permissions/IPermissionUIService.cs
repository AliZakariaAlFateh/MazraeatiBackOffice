using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration.Permissions
{
    public interface IPermissionUIService
    {
        Task<bool> HasPermissionAsync(int userId, string screenUrl, string action);
        Task<bool> CanViewAsync(int userId, string screenUrl);
        Task<bool> CanCreateAsync(int userId, string screenUrl);
        Task<bool> CanEditAsync(int userId, string screenUrl);
        Task<bool> CanDeleteAsync(int userId, string screenUrl);
        Task<bool> CanExportAsync(int userId, string screenUrl);

        #region //New Methods 

        Task<bool> HasPermissionAsync(string screenUrl, string action);
        Task<bool> CanViewAsync(string screenUrl);
        Task<bool> CanCreateAsync(string screenUrl);
        Task<bool> CanEditAsync(string screenUrl);
        Task<bool> CanDeleteAsync(string screenUrl);
        Task<bool> CanExportAsync(string screenUrl);

        #endregion


    }
}
