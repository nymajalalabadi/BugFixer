using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.Application.Statics;
using BugFixer.domain.ViewModels.Admin.User;
using BugFixer.Web.Extentions;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
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

        #region Change User Avatar

        public async Task<IActionResult> ChangeUserAvatar(IFormFile userAvatar, long userId)
        {
            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(userAvatar.FileName);

            var result = userAvatar.AddImageToServer(fileName, PathTools.UserAvatarServerPath, 150, 150, PathTools.UserAvatarServerThumb);

            if (!result)
            {
                return JsonResponseStatus.Error();
            }

            await _userService.ChangeAvatarForUser(userId, fileName);

            TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";

            return JsonResponseStatus.Success();
        }

        #endregion

    }
}
