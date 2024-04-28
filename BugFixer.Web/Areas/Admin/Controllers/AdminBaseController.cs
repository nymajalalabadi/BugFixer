using BugFixer.Web.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [PermissionChecker(1)]
    public class AdminBaseController : Controller
    {
        public static readonly string SuccessMessage = "SuccessMessage";
        public static readonly string WarningMessage = "WarningMessage";
        public static readonly string InfoMessage = "InfoMessage";
        public static readonly string ErrorMessage = "ErrorMessage";
    }
}
