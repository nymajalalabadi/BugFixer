using BugFixer.domain.ViewModels.UserPanel.Account;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class AccountController : UserPanelBaseController
    {
        #region constractor



        #endregion

        #region edit user info

        [HttpGet]
        public async Task<IActionResult> EditInfo()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(EditUserViewModel edit)
        {
            return View();
        }

        #endregion
    }
}
