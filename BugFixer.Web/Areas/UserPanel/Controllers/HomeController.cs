using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.Statics;
using BugFixer.Web.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class HomeController : UserPanelBaseController
    {
        #region constractor

        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        #region change user avatar

        public async Task<IActionResult> ChangeUserAvatar(IFormFile userAvatar)
        {
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(userAvatar.FileName);

            var result = userAvatar.AddImageToServer(fileName, PathTools.UserAvatarServerPath, 150, 150, PathTools.UserAvatarThumb);

            if (!result)
            {
                return JsonResponseStatus.Error();
            }

            await _userService.ChangeAvatarForUser(User.GetUserId(), fileName);

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";

            return JsonResponseStatus.Success();
        }

        #endregion
    }
}
