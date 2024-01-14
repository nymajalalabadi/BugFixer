using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Controllers
{
    public class BaseController : Controller
    {
        public static readonly string SuccessMessage = "SuccessMessage";
        public static readonly string WarningMessage = "WarningMessage";
        public static readonly string InfoMessage = "InfoMessage";
        public static readonly string ErrorMessage = "ErrorMessage";
    }
}
