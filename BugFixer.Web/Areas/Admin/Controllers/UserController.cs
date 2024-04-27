using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.Admin.User;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        #region constractor

        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        #endregion

        #region User List

        public async Task<IActionResult> FilterUsers(FilterUserAdminViewModel filter)
        {
            var result = await _userService.FilterUserAdmin(filter);

            return View(result);
        }

        #endregion

        #region Edit User

        [HttpGet]
        public async Task<IActionResult> EditUserInfo(long userId)
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserInfo(EditUserAdminViewModel edit)
        {
            return View(edit);
        }

        #endregion
    }
}
