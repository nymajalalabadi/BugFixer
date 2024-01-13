using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region constractor

        public AccountController()
        {
            
        }

        #endregion

        #region login

        public IActionResult Login()
        {
            return View();
        }

        #endregion

        #region register

        public IActionResult Register()
        {
            return View();
        }

        #endregion
    }
}
