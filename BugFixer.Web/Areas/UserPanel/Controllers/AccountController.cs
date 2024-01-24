using BugFixer.Application.Extensions;
using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.ViewModels.UserPanel.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BugFixer.Web.Areas.UserPanel.Controllers
{
    public class AccountController : UserPanelBaseController
    {
        #region constractor

        private readonly IStateService _stateService;

        private readonly IUserService _userService;

        public AccountController(IStateService stateService, IUserService userService)
        {
            _stateService = stateService;
            _userService = userService;
        }

        #endregion

        #region edit user info

        [HttpGet]
        public async Task<IActionResult> EditInfo()
        {
            ViewData["states"] = await _stateService.GetAllStates();

            var result = await _userService.FillEditUserViewModel(User.GetUserId());

            if (result.CountryId.HasValue)
            {
                ViewData["Cities"] = await _stateService.GetAllStates(result.CountryId.Value);
            }

            return View(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(EditUserViewModel edit)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserInfo(edit, User.GetUserId());

                switch (result)
                {
                    case EditUserInfoResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                        return RedirectToAction("EditInfo", "Account", new { area = "UserPanel" });

                    case EditUserInfoResult.NotValidDate:
                        TempData[ErrorMessage] = "تاریخ وارد شده معتبر نمی باشد .";
                        break;
                }
            }

            ViewData["States"] = await _stateService.GetAllStates();

            return View(edit);
        }

        #endregion

        #region load cities

        public async Task<IActionResult> LoadCities(long countryId)
        {
            var result = await _stateService.GetAllStates(countryId);

            return new JsonResult(result); 
        }

        #endregion

        #region change user password

        [HttpGet]
        public async Task<IActionResult> ChangeUserPassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(ChnageUserPasswordViewModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePasswordForUser(User.GetUserId(), changePassword);

                switch (result)
                {
                    case ChangeUserPasswordResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد .";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Account", new { area = "" });

                    case ChangeUserPasswordResult.OldPasswordIsNotValid:
                        ModelState.AddModelError("OldPassword", "کلمه عبور وارد شده اشتباه است .");
                        break;
                }
            }

            return View(changePassword);
        }

        #endregion
    }
}
