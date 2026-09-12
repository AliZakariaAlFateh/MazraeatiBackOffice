using MazraeatiBackOffice.Configuration.Permissions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration.TagHelpers
{
    public class PermissionTagHelper
    {
        [HtmlTargetElement("div", Attributes = "permission")]
        [HtmlTargetElement("button", Attributes = "permission")]
        [HtmlTargetElement("a", Attributes = "permission")]
        [HtmlTargetElement("span", Attributes = "permission")]
        [HtmlTargetElement("i", Attributes = "permission")]
        [HtmlTargetElement("input", Attributes = "permission")]
        [HtmlTargetElement("select", Attributes = "permission")]
        public class PermissionTagHelper : TagHelper
        {
            private readonly IPermissionUIService _permissionUIService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public PermissionTagHelper(
                IPermissionUIService permissionUIService,
                IHttpContextAccessor httpContextAccessor)
            {
                _permissionUIService = permissionUIService;
                _httpContextAccessor = httpContextAccessor;
            }

            [HtmlAttributeName("permission")]
            public string Permission { get; set; }

            [HtmlAttributeName("permission-action")]
            public string PermissionAction { get; set; } = "view";

            [HtmlAttributeName("permission-hide")]
            public bool PermissionHide { get; set; } = true;

            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity.IsAuthenticated)
                {
                    if (PermissionHide) output.SuppressOutput();
                    return;
                }

                var userIdClaim = user.FindFirst("UserId")?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                {
                    if (PermissionHide) output.SuppressOutput();
                    return;
                }

                // SuperAdmin يشوف كل حاجة
                var isSuperAdmin = user.FindFirst("IsSuperAdmin")?.Value == "true";
                if (isSuperAdmin) return;

                var hasPermission = await _permissionUIService.HasPermissionAsync(
                    userId, Permission, PermissionAction);

                if (!hasPermission)
                {
                    if (PermissionHide)
                    {
                        output.SuppressOutput();
                    }
                    else
                    {
                        output.Attributes.SetAttribute("disabled", "disabled");
                        output.Attributes.SetAttribute("style", "opacity: 0.5; cursor: not-allowed;");
                    }
                }
            }
        }
    }
}
