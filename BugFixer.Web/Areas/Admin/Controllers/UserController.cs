using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Implementations;
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

        private readonly IStateService _stateService;

        public UserController(IUserService userService, IStateService stateService)
        {
            _userService = userService;
            _stateService = stateService;
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
            var result = await _userService.FillEditUserAdminViewModel(userId);

            if (result == null) return NotFound();

            ViewData["States"] = await _stateService.GetAllStates();

            if (result.CountryId.HasValue)
            {
                ViewData["Cities"] = await _stateService.GetAllStates(result.CountryId.Value);
            }

            return View(result);

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserInfo(EditUserAdminViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نمی باشد";

                ViewData["States"] = await _stateService.GetAllStates();

                if (edit.CountryId.HasValue)
                {
                    ViewData["Cities"] = await _stateService.GetAllStates(edit.CountryId.Value);
                }

                return View(edit);
            }

            var editResult = await _userService.EditUserAdmin(edit);

            switch (editResult)
            {
                case EditUserAdminResult.Success:
                    TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                    return RedirectToAction("FilterUsers", "User", new { area = "Admin" });

                case EditUserAdminResult.NotValidEmail:
                    ModelState.AddModelError("Email", "ایمیل وارد شده از قبل موجود است");
                    break;

                case EditUserAdminResult.UserNotFound:
                    TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                    return RedirectToAction("FilterUsers", "User", new { area = "Admin" });
            }

            ViewData["States"] = await _stateService.GetAllStates();

            if (edit.CountryId.HasValue)
            {
                ViewData["Cities"] = await _stateService.GetAllStates(edit.CountryId.Value);
            }

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
