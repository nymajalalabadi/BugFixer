using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.ViewComponents
{
    public class UserMainMenuBoxViewComponent : ViewComponent
    {
        #region constractor

        private readonly IUserService _userService;

        public UserMainMenuBoxViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userService.GetUserById(User.GetUserId());

            return View("UserMainMenuBox", user);
        }
    }
}
