using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.Entities.Account;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.ViewComponents
{
    public class UserSideBarViewComponent : ViewComponent
    {
        #region constractor

        private readonly IUserService _userService;

        public UserSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _userService.GetUserById(User.GetUserId());

            return View("UserSideBar", result);
        }
    }
}
