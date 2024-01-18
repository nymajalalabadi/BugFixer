using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class UserPanelBaseController : Controller
    {
        public static readonly string SuccessMessage = "SuccessMessage";
        public static readonly string WarningMessage = "WarningMessage";
        public static readonly string InfoMessage = "InfoMessage";
        public static readonly string ErrorMessage = "ErrorMessage";
    }
}
