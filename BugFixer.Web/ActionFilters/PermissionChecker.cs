using BugFixer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BugFixer.Application.Extensions;

namespace BugFixer.Web.ActionFilters
{
    public class PermissionChecker : Attribute, IAsyncAuthorizationFilter
    {
        private readonly long _permissionId;

        public PermissionChecker(long permissionId)
        {
            _permissionId = permissionId;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService))!;

            if (!await userService.CheckUserPermission(_permissionId, context.HttpContext.User.GetUserId()))
            {
                context.Result = new RedirectResult("/");
            }
        }
    }

}
